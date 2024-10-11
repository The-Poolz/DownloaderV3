using FluentAssertions;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Resolvers;

namespace DownloaderV3.Source.CovalentDocument.Tests.Resolvers;

public class ValueResolversTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldsCorrectly()
    {
        var downloaderSettings = new DownloaderSettings { StartingBlock = 100, EndingBlock = 200, MaxBatchSize = 300 };

        var resolver = new CalculatedValueResolver(400, 50, downloaderSettings);

        resolver.CalculateEndingBlock.Should().Be(350);
        resolver.IsValidateStartingBlock.Should().BeTrue();
    }

    [Fact]
    public void IsValidateStartingBlock_ShouldReturnFalse_WhenStartingBlockIsNotValid()
    {
        var downloaderSettings = new DownloaderSettings { StartingBlock = 300 };
        var resolver = new CalculatedValueResolver(290, 10, downloaderSettings);

        var result = resolver.IsValidateStartingBlock;

        result.Should().BeFalse();
    }

    [Fact]
    public void CalculateEndingBlock_ShouldReturnCorrectValue_WhenValidSettings()
    {
        var downloaderSettings = new DownloaderSettings { StartingBlock = 100, MaxBatchSize = 500, ChainId = 1 };
        var lastBlockDictionary = new Dictionary<long, long> { { 1, 1000 } };
        var chainSettings = new Dictionary<long, ChainInfo> { { 1, new ChainInfo { BlockPerSecond = 1, DownloadTimeDelay = 50 } } };

        var resolver = new CalculatedValueResolver(lastBlockDictionary[downloaderSettings.ChainId], Convert.ToInt64(chainSettings[downloaderSettings.ChainId].BlockPerSecond * chainSettings[downloaderSettings.ChainId].DownloadTimeDelay), downloaderSettings);

        resolver.EndingBlock.Should().Be(600);
    }

    [Theory]
    [InlineData(100, 400, 50, 150)]
    [InlineData(100, 400, 320, 390)]
    public void ComputeLastBlock_ShouldCalculateCorrectly(long startingBlock, long lastBlockOnCovalent, long batchSize, long expected)
    {
        var downloaderSettings = new DownloaderSettings { StartingBlock = startingBlock, MaxBatchSize = batchSize };
        var resolver = new CalculatedValueResolver(lastBlockOnCovalent, 10, downloaderSettings);

        var result = resolver.CalculateEndingBlock;

        result.Should().Be(expected);
    }

    [Fact]
    public void ComputeLastBlock_ShouldReturnCorrectValue()
    {
        var downloaderSettings = new DownloaderSettings { StartingBlock = 100, MaxBatchSize = 200 };
        var resolver = new CalculatedValueResolver(500, 10, downloaderSettings);

        var result = resolver.ComputeLastBlock;

        var expectedLastBlock = Math.Min(100 + 200, 500 - 10);
        result.Should().Be(expectedLastBlock);
    }

    [Fact]
    public void BatchSize_ShouldReturnCorrectValue_WhenMaxBatchSizeIsLessThanMaxDelta()
    {
        var downloaderSettings = new DownloaderSettings { MaxBatchSize = 5000 };
        var resolver = new CalculatedValueResolver(10000, 100, downloaderSettings);

        var result = resolver.BatchSize;

        result.Should().Be(5000);
    }

    [Fact]
    public void CalculateEndingBlock_ShouldReturnStartingBlock_WhenLastBlockInDictionaryIsZero()
    {
        var downloaderSettings = new DownloaderSettings { StartingBlock = 500, MaxBatchSize = 500, ChainId = 1 };
        var lastBlockDictionary = new Dictionary<long, long> { { 1, 0 } };
        var chainSettings = new Dictionary<long, ChainInfo> { { 1, new ChainInfo { BlockPerSecond = 1, DownloadTimeDelay = 100 } } };

        var resolver = new CalculatedValueResolver(lastBlockDictionary[downloaderSettings.ChainId], Convert.ToInt64(chainSettings[downloaderSettings.ChainId].BlockPerSecond * chainSettings[downloaderSettings.ChainId].DownloadTimeDelay), downloaderSettings);

        resolver.EndingBlock.Should().Be(downloaderSettings.StartingBlock);
    }

    [Fact]
    public void CalculateEndingBlock_ShouldHandleEdgeCase_WhenMaxBatchSizeIsAtLimit()
    {
        var downloaderSettings = new DownloaderSettings { StartingBlock = 200, MaxBatchSize = 10000, ChainId = 1 };
        var lastBlockDictionary = new Dictionary<long, long> { { 1, 10205 } };
        var chainSettings = new Dictionary<long, ChainInfo> { { 1, new ChainInfo { BlockPerSecond = 1, DownloadTimeDelay = 5 } } };

        var resolver = new CalculatedValueResolver(lastBlockDictionary[downloaderSettings.ChainId], Convert.ToInt64(chainSettings[downloaderSettings.ChainId].BlockPerSecond * chainSettings[downloaderSettings.ChainId].DownloadTimeDelay), downloaderSettings);

        resolver.EndingBlock.Should().Be(10200);
    }
}