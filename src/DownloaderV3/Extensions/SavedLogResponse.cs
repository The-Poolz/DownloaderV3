using DownloaderV3.LogRouter;
using DownloaderV3.Destination;

namespace DownloaderV3.Extensions;

public class SavedLogResponse : List<ILogResponse>
{
    public void LockedSaveAll(BaseDestination destination)
    {
        ForEach(response =>
        {
            lock (destination)
            {
                response.Save(destination);
            }
        });
    }
}