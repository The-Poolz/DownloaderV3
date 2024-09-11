using DownloaderV3.Destination;
using Microsoft.EntityFrameworkCore;
using DownloaderV3.Destination.Models;

namespace DownloaderV3.Helpers;

public class SettingDownloader
{
    public List<DownloaderSettings> DownloaderSettings { get; }
    public Dictionary<long, ChainInfo> ChainSettings { get; }

    public SettingDownloader(BaseDestination destination)
    {
        DownloaderSettings = destination.DownloaderSettings
            .Include(ds => ds.DownloaderMappings)
            .Where(ds => ds.Active)
            .ToList();

        ChainSettings = destination.ChainsInfo
            .ToDictionary(c => c.ChainId, c => c);
    }
}