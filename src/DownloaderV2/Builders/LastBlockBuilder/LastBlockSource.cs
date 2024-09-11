using SourceLastBlock.SourcePage;

namespace DownloaderV2.Builders.LastBlockBuilder;

public class LastBlockSource(GetSourcePage getLastBlock)
{
    private readonly Lazy<Dictionary<long, long>> _lastBlockDictionary = new Lazy<Dictionary<long, long>>(getLastBlock.FetchData);

    public Dictionary<long, long> LastBlockDictionary => _lastBlockDictionary.Value;
}