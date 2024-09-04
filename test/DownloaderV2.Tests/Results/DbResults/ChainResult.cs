using DownloaderContext.Models;

namespace DownloaderV2.Tests.Results.DbResults;

public class ChainResult
{
    public static readonly List<ChainInfo> ChainList = new()
    {
        new ChainInfo
        {
            ChainId = 56,
            BlockPerSecond = 0.333f,
            SecondsToWarning = 1000,
            SecondsToError  = 1000,
            DownloadTimeDelay = 60
        },
        new ChainInfo
        {
            ChainId = 1,
            BlockPerSecond = 0.067f,
            SecondsToWarning = 600,
            SecondsToError  = 600,
            DownloadTimeDelay = 60
        }
    };

    public static readonly Dictionary<long, ChainInfo> ChainSettings = new()
    {
        { 56, ChainList[0] },
        { 1, ChainList[1] }
    };
}