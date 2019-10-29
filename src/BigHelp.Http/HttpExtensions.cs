using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BigHelp.Http
{
    public static class HttpExtensions
    {
        public static Task<string> DownloadFileAsync(this HttpClient client, string requestUri, string downloadLocation = null, string fileName = null, IProgress<HttpDownloadProgress> reportCallback = null)
        {
            return DownloadFileAsync(client, new Uri(requestUri), CancellationToken.None, downloadLocation, fileName, reportCallback);
        }
        public static Task<string> DownloadFileAsync(this HttpClient client, string requestUri, CancellationToken cancellationToken, string downloadLocation = null, string fileName = null, IProgress<HttpDownloadProgress> reportCallback = null)
        {
            return DownloadFileAsync(client, new Uri(requestUri), cancellationToken, downloadLocation, fileName, reportCallback);
        }
        public static Task<string> DownloadFileAsync(this HttpClient client, Uri requestUri, string downloadLocation = null, string fileName = null, IProgress<HttpDownloadProgress> reportCallback = null)
        {
            return DownloadFileAsync(client, requestUri, CancellationToken.None, downloadLocation, fileName, reportCallback);
        }
        public static async Task<string> DownloadFileAsync(this HttpClient client, Uri requestUri, CancellationToken cancellationToken, string downloadLocation = null, string fileName = null, IProgress<HttpDownloadProgress> reportCallback = null)
        {
            var respHead = await client.HeadAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            respHead.EnsureSuccessStatusCode();

            if (fileName == null) fileName = respHead.Content.Headers.ContentDisposition?.FileName ?? throw new ArgumentNullException(nameof(fileName), $"{nameof(fileName)} is null and ContentDisposition does not contains a FileName");
            if (downloadLocation == null) downloadLocation = Environment.CurrentDirectory;

            string fullPath = Path.Combine(downloadLocation, fileName);

            HttpResponseMessage resp;

            bool allowResume = respHead.Headers.AcceptRanges?.Contains("bytes") ?? false;
            if (allowResume && File.Exists(fullPath))
            {
                var fi = new FileInfo(fullPath);
                var size = fi.Length;

                if (size < respHead.Content.Headers.ContentLength)
                {
                    var req = new HttpRequestMessage(HttpMethod.Get, requestUri);
                    req.Headers.Range = new RangeHeaderValue(size, null)
                    {
                        Unit = "bytes"
                    };

                    resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                }
                else
                {
                    if (File.Exists(fullPath)) File.Delete(fullPath);
                    resp = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                }
            }
            else
            {
                if (File.Exists(fullPath)) File.Delete(fullPath);
                resp = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            }
            resp.EnsureSuccessStatusCode();

            if (resp.Content == null)
            {
                Debugger.Break();
                return null;
            }

            if (File.Exists(fullPath))
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.None, 8192, true))
                {
                    await resp.DownloadAsync(fileStream, cancellationToken, reportCallback);
                }
            }
            else
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    await resp.DownloadAsync(fileStream, cancellationToken, reportCallback);
                }
            }
            return fullPath;
        }

        public static Task DownloadAsync(this HttpResponseMessage response, Stream destinationStream, IProgress<HttpDownloadProgress> reportCallback = null)
        {
            return DownloadAsync(response, destinationStream, CancellationToken.None, reportCallback);
        }
        public static async Task DownloadAsync(this HttpResponseMessage response, Stream destinationStream, CancellationToken cancellationToken, IProgress<HttpDownloadProgress> reportCallback = null)
        {
            using (var networkStream = await response.Content.ReadAsStreamAsync())
            {
                var progress = new HttpDownloadProgress
                {
                    TotalSize = response.Content.Headers.ContentLength,
                    TotalDownloaded = response.Content.Headers.ContentRange?.From ?? 0L
                };
                var readCount = 0L;
                var buffer = new byte[8192];
                var isFinished = false;

                progress.TotalSize += progress.TotalDownloaded;

                do
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    if (bytesRead == 0)
                    {
                        isFinished = true;
                        reportCallback?.Report(progress);
                        continue;
                    }

                    await destinationStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);

                    progress.TotalDownloaded += bytesRead;
                    readCount += 1;

                    if (readCount % 100 == 0) reportCallback?.Report(progress);
                }
                while (!isFinished);
            }
        }


        public static Task<HttpResponseMessage> HeadAsync(this HttpClient client, Uri requestUri, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            return client.SendAsync(new HttpRequestMessage(HttpMethod.Head, requestUri), completionOption, cancellationToken);
        }

        public static async Task<HttpRequestMessage> CloneIfAlreadySent(this HttpRequestMessage httpRequestMessage)
        {
            var fInfo = GetInternalSendStatus();
            if (fInfo != null)
            {
                var sent = Convert.ToInt32(fInfo.GetValue(httpRequestMessage)) != 0;
                if (!sent) return httpRequestMessage;
            }

            var clonedRequest = new HttpRequestMessage(httpRequestMessage.Method, httpRequestMessage.RequestUri)
            {
                Content = await httpRequestMessage.Content.CloneContent(),
                Version = httpRequestMessage.Version
            };
            foreach (KeyValuePair<string, object> prop in httpRequestMessage.Properties)
            {
                clonedRequest.Properties.Add(prop);
            }
            foreach (KeyValuePair<string, IEnumerable<string>> header in httpRequestMessage.Headers)
            {
                clonedRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            return clonedRequest;
        }
        private static async Task<HttpContent> CloneContent(this HttpContent content)
        {
            if (content == null) return null;

            using (var ms = new MemoryStream())
            {
                await content.CopyToAsync(ms);
                ms.Position = 0;

                var clone = new StreamContent(ms);
                foreach (KeyValuePair<string, IEnumerable<string>> header in content.Headers)
                {
                    clone.Headers.Add(header.Key, header.Value);
                }
                return clone;
            }
        }

        private static FieldInfo _sendStatusFieldInfo;
        private static bool _initializedSendStatusFieldInfo;
        private static FieldInfo GetInternalSendStatus()
        {
            //https://www.lifehacker.com.au/2016/05/microsoft-reconsiders-reflection-serialisation-changes-for-net-core/
            //https://devblogs.microsoft.com/dotnet/evolving-the-reflection-api/

            if (_initializedSendStatusFieldInfo) return _sendStatusFieldInfo;

            Type requestType = typeof(HttpRequestMessage);
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            //https://github.com/microsoft/referencesource/blob/master/System/net/System/Net/Http/HttpRequestMessage.cs
            //https://github.com/mono/mono/blob/master/mcs/class/System.Net.Http/System.Net.Http/HttpRequestMessage.cs
            _sendStatusFieldInfo = requestType.GetField("_sendStatus", bindingFlags)
                                     ?? requestType.GetField("sendStatus", bindingFlags)
                                     ?? requestType.GetField("is_used", bindingFlags);

            _initializedSendStatusFieldInfo = true;
            return _sendStatusFieldInfo;
        }
    }
}