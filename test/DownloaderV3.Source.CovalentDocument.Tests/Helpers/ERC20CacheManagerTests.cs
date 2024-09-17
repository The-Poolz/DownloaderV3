using Moq;
using System.Numerics;
using FluentAssertions;
using Net.Cache.DynamoDb.ERC20.Models;
using DownloaderV3.Source.CovalentDocument.Helpers;

namespace DownloaderV3.Source.CovalentDocument.Tests.Helpers;

public class ERC20CacheManagerTests
{
    [Fact]
    public void GetTokenInfo_ShouldReturnTokenInfoFromCache()
    {
        const string tokenAddress = "0x1234567890abcdef1234567890abcdef12345678";
        const long chainId = 1;
        const string name = "Test Token";
        const string symbol = "TTK";
        const byte decimals = 6;
        var totalSupply = new BigInteger(1000000);

        var expectedTokenInfo = new ERC20DynamoDbTable(chainId, tokenAddress, name, symbol, decimals, totalSupply);

        var mockCacheManager = new Mock<ERC20CacheManager>();
        mockCacheManager.Setup(x => x.GetTokenInfo(It.IsAny<long>(), It.IsAny<string>()))
            .Returns(expectedTokenInfo);

        var result = mockCacheManager.Object.GetTokenInfo(chainId, tokenAddress);

        result.Should().BeEquivalentTo(expectedTokenInfo);

        mockCacheManager.Verify(x => x.GetTokenInfo(chainId, tokenAddress), Times.Once);
    }
}