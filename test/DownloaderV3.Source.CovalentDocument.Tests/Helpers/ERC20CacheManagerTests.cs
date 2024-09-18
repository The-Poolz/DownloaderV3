using Moq;
using System.Numerics;
using FluentAssertions;
using Net.Cache.DynamoDb.ERC20;
using Net.Cache.DynamoDb.ERC20.Models;
using DownloaderV3.Source.CovalentDocument.Helpers;

namespace DownloaderV3.Source.CovalentDocument.Tests.Helpers;

public class ERC20CacheManagerTests
{
    public ERC20CacheManagerTests()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-east-1");
        Environment.SetEnvironmentVariable("LastBlockKey", "test");
        Environment.SetEnvironmentVariable("ApiUrl", "test");
    }
    [Fact]
    public void GetTokenInfo_ShouldReturnTokenInfoFromCache()
    {
        const string tokenAddress = "0x1234567890abcdef1234567890abcdef12345678";
        const long chainId = 1;
        const string name = "Test Token";
        const string symbol = "TTK";
        const byte decimals = 6;
        var totalSupplyWei = BigInteger.Parse("1000000000000");

        var expectedTokenInfo = new ERC20DynamoDbTable(chainId, tokenAddress, name, symbol, decimals, totalSupplyWei);

        var mockCacheProvider = new Mock<ERC20CacheProvider>();
        mockCacheProvider.Setup(x => x.GetOrAdd(It.IsAny<GetCacheRequest>()))
            .Returns(expectedTokenInfo);

        var cacheManager = new ERC20CacheManager(mockCacheProvider.Object);

        var result = cacheManager.GetTokenInfo(chainId, tokenAddress);

        result.ChainId.Should().Be(chainId);
        result.Address.Should().Be(tokenAddress);
        result.Name.Should().Be(name);
        result.Symbol.Should().Be(symbol);
        result.Decimals.Should().Be(decimals);
        result.TotalSupply.Should().Be((decimal)1000000);
    }
}