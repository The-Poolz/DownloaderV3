using DownloaderContext;
using DownloaderContext.Models;
using Microsoft.EntityFrameworkCore;

namespace DownloaderV3.Helpers;

public class SettingDownloader
{
    public List<DownloaderSettings> DownloaderSettings { get; }
    public Dictionary<long, ChainInfo> ChainSettings { get; }

    public SettingDownloader(BaseDownloaderContext context)
    {
        DownloaderSettings = context.DownloaderSettings
            .Include(ds => ds.DownloaderMappings)
            .Where(ds => ds.Active)
            .ToList();

        ChainSettings = context.ChainsInfo
            .ToDictionary(c => c.ChainId, c => c);
    }
}