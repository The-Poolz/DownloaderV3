using DownloaderV2.Builders.LastBlockBuilder.SourcePage;

namespace DownloaderV2.Builders.LastBlockBuilder;

public class LastBlockDownloader(GetSourcePage getLastBlock)
{
    private Task<Dictionary<long, long>>? _lastBlockDictionary;

    public Task<Dictionary<long, long>> LastBlockDictionary => _lastBlockDictionary ??= getLastBlock.FetchDataAsync();
}