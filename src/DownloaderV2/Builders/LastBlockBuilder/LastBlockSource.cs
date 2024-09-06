using DownloaderV2.Builders.LastBlockBuilder.SourcePage;

namespace DownloaderV2.Builders.LastBlockBuilder;

public class LastBlockSource
{
    private static LastBlockSource? _instance;
    private static readonly object Lock = new object();
    private Task<Dictionary<long, long>>? _lastBlockDictionary;
    private readonly GetSourcePage _getLastBlock;

    private LastBlockSource(GetSourcePage getLastBlock) => _getLastBlock = getLastBlock;

    public static LastBlockSource GetInstance(GetSourcePage getLastBlock)
    {
        lock (Lock)
        {
            _instance ??= new LastBlockSource(getLastBlock);
        }
        return _instance;
    }

    public Task<Dictionary<long, long>> LastBlockDictionary => _lastBlockDictionary ??= _getLastBlock.FetchDataAsync();
}