using DownloaderContext;

namespace DownloaderV2.Builders.DownloadHandlerBuilders;

public class DownloadHandlerFactory : IDownloadHandlerFactory
{
    public IDownloadHandler Create(BaseDownloaderContext context)
    {
        return new DownloadHandler(context);
    }
}