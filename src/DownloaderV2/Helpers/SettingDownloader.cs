using DownloaderContext.Models;
using Microsoft.EntityFrameworkCore;

namespace DownloaderV2.Helpers;

public class SettingDownloader
{
    public List<DownloaderSettings> DownloaderSettings { get; }
    public Dictionary<long, ChainInfo> ChainSettings { get; }

    public SettingDownloader(DbContext context)
    {
        DownloaderSettings = context.Set<DownloaderSettings>()
            .Include(ds => ds.DownloaderMappings)
            .Where(ds => ds.Active)
            .ToList();

        ChainSettings = context.Set<ChainInfo>()
            .ToDictionary(c => c.ChainId, c => c);
    }
}