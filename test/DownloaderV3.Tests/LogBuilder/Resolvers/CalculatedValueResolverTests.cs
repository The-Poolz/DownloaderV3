﻿using FluentAssertions;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Resolvers;

namespace DownloaderV3.Tests.LogBuilder.Resolvers;

public class CalculatedValueResolverTests
{
    [Fact]
    public void CalculateEndingBlock_ShouldReturnCorrectValue_WhenValidSettings()
    {
        var downloaderSettings = new DownloaderSettings { StartingBlock = 100, MaxBatchSize = 500, ChainId = 1 };
        var lastBlockDictionary = new Dictionary<long, long> { { 1, 1000 } };
        var chainSettings = new Dictionary<long, ChainInfo> { { 1, new ChainInfo { BlockPerSecond = 1, DownloadTimeDelay = 50 } } };

        var resolver = new CalculatedValueResolver(lastBlockDictionary[downloaderSettings.ChainId], Convert.ToInt64(chainSettings[downloaderSettings.ChainId].BlockPerSecond * chainSettings[downloaderSettings.ChainId].DownloadTimeDelay), downloaderSettings);

        resolver.EndingBlock.Should().Be(600);
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
    public void CalculateEndingBlock_ShouldAdjustToStartingBlock_WhenCalculatedEndBlockLessThanStartingBlock()
    {
        var downloaderSettings = new DownloaderSettings { StartingBlock = 500, MaxBatchSize = 500, ChainId = 1 };
        var lastBlockDictionary = new Dictionary<long, long> { { 1, 550 } };
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