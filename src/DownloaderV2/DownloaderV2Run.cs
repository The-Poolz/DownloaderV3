using DownloaderContext;
using DownloaderV2.Result;
using DownloaderV2.Helpers;
using DownloaderV2.Builders.DownloadHandlerBuilders;

namespace DownloaderV2
{
    public class DownloaderV2Run(BaseDownloaderContext context, IDownloadHandlerFactory factory)
    {
        public async Task<IEnumerable<ResultObject>> RunAsync()
        {
            ApplicationLogger.Initialize();
            var handler = factory.Create(context);
            return await handler.HandleAsync();
        }
    }
}