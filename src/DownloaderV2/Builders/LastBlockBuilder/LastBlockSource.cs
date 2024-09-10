using SourceLastBlock.SourcePage;

namespace DownloaderV2.Builders.LastBlockBuilder
{
    public class LastBlockSource(GetSourcePage getLastBlock)
    {
        private readonly Lazy<Task<Dictionary<long, long>>> _lastBlockDictionary = new Lazy<Task<Dictionary<long, long>>>(getLastBlock.FetchDataAsync);

        public Task<Dictionary<long, long>> LastBlockDictionary => _lastBlockDictionary.Value;
    }
}