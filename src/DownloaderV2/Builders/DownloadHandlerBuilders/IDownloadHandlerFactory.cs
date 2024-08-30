using DownloaderContext;

namespace DownloaderV2.Builders.DownloadHandlerBuilders;

public interface IDownloadHandlerFactory
{
    IDownloadHandler Create(BaseDownloaderContext context);
}