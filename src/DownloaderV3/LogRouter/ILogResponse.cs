using DownloaderContext;

namespace DownloaderV3.LogRouter;

public interface ILogResponse
{
    public void Save(BaseDownloaderContext context);
}