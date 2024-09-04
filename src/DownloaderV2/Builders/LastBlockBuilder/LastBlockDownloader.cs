using DownloaderV2.Builders.LastBlockBuilder.LastBlockService;

namespace DownloaderV2.Builders.LastBlockBuilder;

public class LastBlockDownloader(ILastBlockService lastBlockService)
{
    private Task<Dictionary<long, long>>? _lastBlockDictionary;

    public Task<Dictionary<long, long>> LastBlockDictionary => _lastBlockDictionary ??= lastBlockService.FetchLastBlockDataAsync();
}