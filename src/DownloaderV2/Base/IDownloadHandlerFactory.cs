using DownloaderContext;

namespace DownloaderV2.Base;

public interface IDownloadHandlerFactory
{
    IDownloadHandler Create(BaseDownloaderContext context);
}