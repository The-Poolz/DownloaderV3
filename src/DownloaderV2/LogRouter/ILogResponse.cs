using Microsoft.EntityFrameworkCore;

namespace DownloaderV2.LogRouter;

public interface ILogResponse
{
    public void Save(DbContext context);
}