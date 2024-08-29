using DownloaderV2.Helpers;
using DownloaderV2.Models.LastBlock;

namespace DownloaderV2.Builders.LastBlockBuilder;

public class LastBlockDownloader : BaseDownloader
{
    public LastBlockDownloader()
    {
        DownloadedLastBlockData = DownloadData?.ToObject<LastBlockResponse>() ?? ApplicationLogger.LogAndThrowDynamic(new InvalidOperationException(ExceptionMessages.FailedToRetrieveLastBlockData));
        MakeLastBlocksDictionary();
    }

    public Dictionary<long, long> LastBlockDictionary { get; } = new();
    private LastBlockResponse DownloadedLastBlockData { get; }
    private void MakeLastBlocksDictionary()
    {
        foreach (var item in DownloadedLastBlockData.Data.Items ?? [])
            LastBlockDictionary[item.ChainId] = item.BlockHeight;
    }
}