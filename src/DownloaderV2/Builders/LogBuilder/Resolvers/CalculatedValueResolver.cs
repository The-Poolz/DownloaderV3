using DownloaderContext.Models;

namespace DownloaderV2.Builders.LogBuilder.Resolvers;

public class CalculatedValueResolver(long lastBlockOnCovalent, long delayBlocks, DownloaderSettings downloaderSettings)
{
    private const int MaxDelta = 10000;
    private readonly long _lastBlockOnCovalent = lastBlockOnCovalent > 0 ? lastBlockOnCovalent: downloaderSettings.EndingBlock;  

    public long EndingBlock => CalculateEndingBlock;
    public bool IsValidateStartingBlock => downloaderSettings.StartingBlock < _lastBlockOnCovalent;

    public long CalculateEndingBlock => Math.Max(ComputeLastBlock, downloaderSettings.StartingBlock);

    public long ComputeLastBlock => Math.Min(downloaderSettings.StartingBlock + BatchSize, _lastBlockOnCovalent - delayBlocks);

    public long BatchSize => Math.Min(downloaderSettings.MaxBatchSize, MaxDelta);
}