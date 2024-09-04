using DownloaderContext;
using DownloaderV2.Result;

namespace DownloaderV2
{
    public class DownloaderV2Run(BaseDownloaderContext context)
    {
        public virtual async Task<IEnumerable<ResultObject>> RunAsync() => await new DownloadHandler(context).HandleAsync();
    }
}