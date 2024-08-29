using DownloaderContext;
using DownloaderV2.Result;
using DownloaderV2.Helpers;

namespace DownloaderV2
{
    public class DownloaderV2Run(BaseDownloaderContext context)
    {
        public async Task<IEnumerable<ResultObject>> RunAsync()
        {
            ApplicationLogger.Initialize();

            return await new DownloadHandler(context).HandleAsync();
        }
    }
}