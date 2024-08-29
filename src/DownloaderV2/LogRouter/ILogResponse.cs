using DownloaderContext;

namespace DownloaderV2.LogRouter;

public interface ILogResponse
{
    public void Save(BaseDownloaderContext context);
}