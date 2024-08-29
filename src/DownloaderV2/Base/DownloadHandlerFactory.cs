using DownloaderContext;

namespace DownloaderV2.Base;

public class DownloadHandlerFactory : IDownloadHandlerFactory
{
    public IDownloadHandler Create(BaseDownloaderContext context)
    {
        return new DownloadHandler(context);
    }
}