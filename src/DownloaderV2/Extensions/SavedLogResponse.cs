using DownloaderV2.LogRouter;
using Microsoft.EntityFrameworkCore;

namespace DownloaderV2.Extensions;

public class SavedLogResponse : List<ILogResponse>
{
    public void LockedSaveAll(DbContext context)
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