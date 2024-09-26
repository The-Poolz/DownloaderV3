using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using DownloaderV3.Destination.Models;

namespace DownloaderV3.Destination.Tests;

public class BaseDestinationTests
{
    private readonly DbContextOptions<BaseDestination> _options = new DbContextOptionsBuilder<BaseDestination>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    [Fact]
    public void BaseDestination_ShouldCreateDbSetsCorrectly()
    {
        using var context = new BaseDestination(_options);

        context.Chains.Should().NotBeNull();
        context.ChainsInfo.Should().NotBeNull();
        context.DownloaderSettings.Should().NotBeNull();
        context.DownloaderMapping.Should().NotBeNull();
    }

    [Fact]
    public void BaseDestination_ShouldSaveAndRetrieveChainEntity()
    {
        using var context = new BaseDestination(_options);
        var chain = new Chain
        {
            ChainId = 1,
            Name = "Ethereum",
            RpcConnection = "https://rpc.url"
        };

        context.Chains.Add(chain);
        context.SaveChanges();

        var savedChain = context.Chains.FirstOrDefault(c => c.ChainId == 1);
        savedChain.Should().NotBeNull();
        savedChain!.ChainId.Should().Be(1);
        savedChain.Name.Should().Be("Ethereum");
        savedChain.RpcConnection.Should().Be("https://rpc.url");
    }

    [Fact]
    public void BaseDestination_ShouldSaveAndRetrieveChainInfoEntity()
    {
        using var context = new BaseDestination(_options);
        var chainInfo = new ChainInfo()
        {
            ChainId = 1,
            BlockPerSecond = 10,
            SecondsToWarning= 12,
            SecondsToError = 20,
            DownloadTimeDelay = 100
        };

        context.ChainsInfo.Add(chainInfo);
        context.SaveChanges();

        var savedChainsInfo = context.ChainsInfo.FirstOrDefault(c => c.ChainId == 1);
        savedChainsInfo.Should().NotBeNull();
        savedChainsInfo!.BlockPerSecond.Should().Be(10);
        savedChainsInfo.SecondsToWarning.Should().Be(12);
        savedChainsInfo.SecondsToError.Should().Be(20);
        savedChainsInfo.DownloadTimeDelay.Should().Be(100);
    }

    [Fact]
    public void BaseDestination_ShouldSaveAndRetrieveDownloaderSettingsEntity()
    {
        using var context = new BaseDestination(_options);

        var settings = new DownloaderSettings
        {
            ChainId = 1,
            StartingBlock = 1,
            EndingBlock = 200,
            MaxBatchSize = 50000,
            ContractAddress = "0x1234567890abcdef",
            EventHash = "0xabcdef",
            Key = "SomeKey",
            ResponseType = "SomeResponse",
            Active = true,
            MaxPageNumber = 1000,
            UrlSet = "https://api/{{ChainId}}/events/address/{{ContractAddress}}/?starting-block={{StartingBlock}}&ending-block={{EndingBlock}}&page-number={{PageNumber}}&page-size={{MaxPageNumber}}&key={{Key}}"
        };

        context.DownloaderSettings.Add(settings);
        context.SaveChanges();

        var downloaderMapping = new DownloaderMapping
        {
            Id = 1,
            Path = "/path/to/resource",
            Converter = "JsonConverter",
            Name = "TestMapping",
            DownloaderSettings = settings
        };

        var savedSettings = context.DownloaderSettings.FirstOrDefault(s => s.ContractAddress == "0x1234567890abcdef");
        savedSettings.Should().NotBeNull();
        savedSettings!.ChainId.Should().Be(1);
        savedSettings.StartingBlock.Should().Be(1);
        savedSettings.EndingBlock.Should().Be(200);
        savedSettings.MaxBatchSize.Should().Be(50000);
        savedSettings.EventHash.Should().Be("0xabcdef");
        savedSettings.Key.Should().Be("SomeKey");
        savedSettings.ResponseType.Should().Be("SomeResponse");
        savedSettings.Active.Should().BeTrue();
        savedSettings.MaxPageNumber.Should().Be(1000);
        savedSettings.UrlSet.Should().Be("https://api/{{ChainId}}/events/address/{{ContractAddress}}/?starting-block={{StartingBlock}}&ending-block={{EndingBlock}}&page-number={{PageNumber}}&page-size={{MaxPageNumber}}&key={{Key}}");

        context.DownloaderMapping.Add(downloaderMapping);
        context.SaveChanges();

        var savedMapping = context.DownloaderMapping.FirstOrDefault(m => m.DownloaderSettings.ContractAddress == "0x1234567890abcdef");

        savedMapping.Should().NotBeNull();
        savedMapping!.Path.Should().Be("/path/to/resource");
        savedMapping.Converter.Should().Be("JsonConverter");
        savedMapping.Name.Should().Be("TestMapping");
    }
}