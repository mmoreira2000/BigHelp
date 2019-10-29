using System;

namespace BigHelp.Http
{
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