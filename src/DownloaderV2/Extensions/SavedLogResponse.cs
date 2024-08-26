using DownloaderContext;
using DownloaderV2.LogRouter;

namespace DownloaderV2.Extensions;

public class SavedLogResponse : List<ILogResponse>
{
    public void LockedSaveAll(BaseDownloaderContext context)
    {
        ForEach(response =>
        {
            lock (context)
            {
                response.Save(context);
            }
        });
    }
}