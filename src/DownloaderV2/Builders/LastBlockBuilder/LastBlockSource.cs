using SourceLastBlock.AbstractClass;

namespace DownloaderV2.Builders.LastBlockBuilder
{
    public class LastBlockSource
    {
        //Done: Changed Lazy initialization to Singleton with constructor-based dependency injection for GetSourcePage realization
        private static LastBlockSource? _instance;
        private readonly Lazy<Task<Dictionary<long, long>>> _lastBlockDictionary;

        private LastBlockSource(GetSourcePage getLastBlock)
        {
            _lastBlockDictionary = new Lazy<Task<Dictionary<long, long>>>(getLastBlock.FetchDataAsync);
        }

        public static void Initialize(GetSourcePage getLastBlock) => _instance ??= new LastBlockSource(getLastBlock);

        public static LastBlockSource Instance =>
            _instance ?? throw new InvalidOperationException("LastBlockSource not initialized.");

        public Task<Dictionary<long, long>> LastBlockDictionary => _lastBlockDictionary.Value;
    }
}
