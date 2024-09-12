using DownloaderV3.Destination;

namespace DownloaderV3.LogRouter;

public interface ILogResponse
{
    public void Save(BaseDestination destination);
}