using DownloaderContext;
using DownloaderV2.Result;
using DownloaderV2.Helpers.Logger;
using DownloaderV2.Builders.DownloadHandlerBuilders;

namespace DownloaderV2
{
    public class DownloaderV2Run(BaseDownloaderContext context, IDownloadHandlerFactory factory)
    {
        public async Task<IEnumerable<ResultObject>> RunAsync()
        {
            ApplicationLogger.Initialize(new ConsoleLogger());

            var handler = factory.Create(context);
            return await handler.HandleAsync();
        }
    }
}