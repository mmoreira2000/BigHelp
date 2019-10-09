using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BigHelp.Http
{
    static class HttpExtensions
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
    }

    public class HttpDownloadProgress
    {
        public long? TotalSize { get; internal set; }
        public long TotalDownloaded { get; internal set; }

        public double PercentDownloaded => TotalSize.HasValue ? Math.Round((double)TotalDownloaded / TotalSize.Value, 4) : -1D;

        internal HttpDownloadProgress()
        {

        }

        public override string ToString()
        {
            var format = "###,###,###,###,###,###,###";
            if (TotalSize.HasValue)
            {
                return $"{TotalDownloaded.ToString(format)}/{TotalSize.Value.ToString(format)} bytes downloaded ({Math.Round(PercentDownloaded * 100, 2)}%)";
            }
            return $"{TotalDownloaded.ToString(format)} bytes downloaded";
        }
    }
}