using Moq;
using Nethereum.Web3;
using System.Numerics;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Net.Web3.EthereumWallet;
using Net.Cache.DynamoDb.ERC20;
using Nethereum.Contracts.Services;
using Net.Cache.DynamoDb.ERC20.Models;
using DownloaderV3.Destination.Models;
using Nethereum.Contracts.ContractHandlers;
using DownloaderV3.Source.CovalentDocument.Helpers;
using DownloaderV3.Source.CovalentDocument.Decoders;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using DownloaderV3.Source.CovalentDocument.Decoders.DataDecoders;
using DownloaderV3.Source.CovalentDocument.Decoders.DecoderHelpers;

namespace DownloaderV3.Tests.Decoders.DataDecoders;

public class DataDecoderFactoryTests
{
    public DataDecoderFactoryTests()
    {
        Environment.SetEnvironmentVariable("AWS_REGION", "us-east-2");
        Environment.SetEnvironmentVariable("ApiUrl", "https://test?");
        Environment.SetEnvironmentVariable("LastBlockKey", "key");

        Mock<IWeb3> mockWeb3 = new();
        Mock<IEthApiContractService> mockEthApiContractService = new();
        Mock<IContractQueryHandler<DecimalsFunction>> mockContractQueryHandler = new();

        mockWeb3.Setup(m => m.Eth).Returns(mockEthApiContractService.Object);
        mockEthApiContractService.Setup(e => e.GetContractQueryHandler<DecimalsFunction>()).Returns(mockContractQueryHandler.Object);
    }
    private const string HexStringTest = "0x000000000000000000000000000000000000000000000005f1e065d9cbb1f107fffffffffffffffffffffffffffffffffffffffffffffffea50e2874a73c000000000000000000000000000000000000000000007a6cfdfc449ddb3923b8d84000000000000000000000000000000000000000000000019b969d845eb4fe3e17ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffc65d00000000000000000000000000000000000000000000000004deb8a55b2c65880000000000000000000000000000000000000000000000000000000000000000";
    private const string RawTopicData = "0x00000000000000000000000041b56bf3b21c53f6394a44a2ff84f1d2bbc27841000000000000000000000000000000000000000000000000000000007fffffff0000000000000000000000000000000000000000000000007fffffffffffffff";
    const string HexStringToArray = "0x0000000000000000000000000000000000000000000000000000000000000020000000000000000000000000000000000000000000000000000000000000000500000000000000000000000000000000000000000000000000000002540be40000000000000000000000000000000000000000000000000000000000657c584a00000000000000000000000000000000000000000000000000000000cb0b1dc400000000000000000000000000000000000000000000000000000002540be40000000000000000000000000000000000000000000000000000000000cb0b1dc4";

    private const string MainNet = "0x000000000000000000000000000000000000000000000000000000000000004000000000000000000000000000000000000000000000000000000000000000600000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002768747470733a2f2f6e66742e706f6f6c7a2e66696e616e63652f6273632f6d657461646174612f00000000000000000000000000000000000000000000000000";
    private const string TestNet = "0x0000000000000000000000000000000000000000000000000000000000000040000000000000000000000000000000000000000000000000000000000000008000000000000000000000000000000000000000000000000000000000000000076e65774261736500000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005068747470733a2f2f6e66742e706f6f6c7a2e66696e616e63652f746573742f6d657461646174612f68747470733a2f2f6e66742e706f6f6c7a2e66696e616e63652f746573742f6d657461646174612f2f617274000000000000000000000000";

    [Theory]
    [InlineData("PoolDeactivated", nameof(HexToString), "PoolDeactivated")]
    [InlineData("56", nameof(StringToInt32), 56)]
    [InlineData("00000000000000000000000041b56bf3b21c53f6394a44a2ff84f1d2bbc27841", nameof(HexToAddress), "0x41b56bf3b21c53f6394a44a2ff84f1d2bbc27841")]
    [InlineData("000000000000000000000000000000000000000000000000000000007fffffff", nameof(HexToInt32), int.MaxValue)]
    [InlineData("0000000000000000000000000000000000000000000000007fffffffffffffff", nameof(HexToLong), long.MaxValue)]
    [InlineData("0000000000000000000000000000000000000000000000000000000000000001", nameof(HexToBool), true)]
    internal void Create(string topicData, string nameofDecoder, object expected)
    {
        var result = new DataDecoderFactory(CreateMapping(nameofDecoder), new JValue(topicData)).Decoder;

        Assert.Equal(expected.ToString(), result.DecodedData.ToString());
    }

    [Fact]
    internal void Create_HexToDecimal()
    {
        const string topicData = "0000000000000000000000000000000000000000000000016A89298AACF85F15";

        var result = new DataDecoderFactory(CreateMapping(nameof(HexToDecimal)), new JValue(topicData)).Decoder;

        Assert.Equal(26.123456789123456789m, result.DecodedData);
    }

    [Theory]
    [InlineData("0000000000000000000000000000000000000000000000000000000063f23eb1", 1676820145)]
    [InlineData("1000000000000000000", 253402300799)]
    internal void Create_TimestampToDateTime(string topicData, long expectedUnixTime)
    {
        var result = new DataDecoderFactory(CreateMapping(nameof(TimestampToDateTime)), new JValue(topicData)).Decoder;

        Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(expectedUnixTime).DateTime, result.DecodedData);
    }

    [Theory]
    [InlineData("16.02.2023 11:36:35")]
    [InlineData("02/16/2023 11:36:35")]
    [InlineData("02/16/2023 11:36:35 AM")]
    internal void Create_StringToDateTime(string topicData)
    {
        var expected = new DateTime(2023, 02, 16, 11, 36, 35, DateTimeKind.Utc);
        var result = new DataDecoderFactory(CreateMapping(nameof(StringToDateTime)), new JValue(topicData)).Decoder;

        Assert.Equal(expected,(DateTime)result.DecodedData);
    }

    [Fact]
    internal void Create_StringToDateTime_ThrowArgumentException()
    {
        const string topicData = "16|02|2023 11|36|35";
        void TestCode() => GetDecoder(CreateMapping(nameof(StringToDateTime)), new JValue(topicData));

        var exception = Assert.Throws<FormatException>(TestCode);
        Assert.Equal($"String '{topicData}' was not recognized as a valid DateTime.", exception.Message);
    }

    [Theory]
    [InlineData("RawDataDecoder(HexToAddress)", "$#1", "0x41b56bf3b21c53f6394a44a2ff84f1d2bbc27841")]
    [InlineData("RawDataDecoder(HexToInt32)", "$#2", int.MaxValue)]
    [InlineData("RawDataDecoder(HexToLong)", "$#3", long.MaxValue)]
    internal void Create_RawDataDecoder(string nameofDecoder, string path, object expected)
    {
        var result = new DataDecoderFactory(CreateMapping(nameofDecoder, path), new JValue(RawTopicData)).Decoder;

        Assert.Equal(expected, result.DecodedData);
    }

    [Theory]
    [InlineData("RawDataDecoder(HexToNumberNegative)", "$#5", -14755L)]
    internal void Create_RawDataDecoderNew(string nameofDecoder, string path, object expected)
    {
        var result = new DataDecoderFactory(CreateMapping(nameofDecoder, path), new JValue(HexStringTest)).Decoder;

        Assert.Equal(expected, result.DecodedData);
    }

    [Theory]
    [InlineData("StaticDataDecoder(0x0000000000000000000000000000000000000000)", "$#1", "0x0000000000000000000000000000000000000000")]
    [InlineData("StaticDataDecoder(special_value)", "$#2", "special_value")]
    internal void Create_SpecialDecoder(string nameofDecoder, string path, string expected)
    {
        var result = new DataDecoderFactory(CreateMapping(nameofDecoder, path), new JValue("{ \"special_data\": \"test\" }")).Decoder;

        Assert.Equal(expected, result.DecodedData);
    }

    [Theory]
    [InlineData("RawDataDecoder(HexToDecimalNegative)", "$#1", 109.662762912571322631)]
    [InlineData("RawDataDecoder(HexToDecimalNegative)", "$#2", -25)]
    internal void Create_RawDataDecoderHexToNumberNegative(string nameofDecoder, string path, decimal expected)
    {
        var result = new DataDecoderFactory(CreateMapping(nameofDecoder, path), new JValue(HexStringTest)).Decoder;

        Assert.Equal(Math.Round(expected, 12), Math.Round(result.DecodedData, 12));
    }

    [Fact]
    internal void Create_RawDataDecoder_ThrowOnTryParseConverterParams()
    {
        static void TestCode() => GetDecoder(CreateMapping("RawDataDecoder", "$#1"), new JValue(RawTopicData));

        var exception = Assert.Throws<InvalidOperationException>(TestCode);
        Assert.Equal("Data decoder with name '' not found.", exception.Message);
    }

    [Fact]
    internal void Create_RawDataDecoder_ThrowWhenInPathDoNotContainsHashSymbol()
    {
        static void TestCode() => GetDecoder(CreateMapping("RawDataDecoder(HexToAddress)", "$"), new JValue(RawTopicData));

        var exception = Assert.Throws<InvalidOperationException>(TestCode);
        Assert.Equal("Invalid 'path' in 'RawDataDecoder' decoder. Expected format: '$data.raw_log_data#3'.", exception.Message);
    }

    [Fact]
    internal void Create_RawDataDecoder_ThrowWhenAfterHashSymbolDoNotContainsIndex()
    {
        static void TestCode() => GetDecoder(CreateMapping("RawDataDecoder(HexToAddress)", "$#"), new JValue(RawTopicData));

        var exception = Assert.Throws<InvalidOperationException>(TestCode);
        Assert.Equal("Invalid 'path' in 'RawDataDecoder' decoder. Expected format: '$data.raw_log_data#3'.", exception.Message);
    }

    [Fact]
    internal void Create_RawDataDecoder_ThrowWhenAfterHashSymbolDoNotIntegerValue()
    {
        static void TestCode() => GetDecoder(CreateMapping("RawDataDecoder(HexToAddress)", "$#NotInt"), new JValue(RawTopicData));

        var exception = Assert.Throws<InvalidOperationException>(TestCode);
        Assert.Equal("Invalid 'path' in 'RawDataDecoder' decoder. Expected format: '$data.raw_log_data#3'.", exception.Message);
    }

    [Fact]
    internal void Create_RawDataDecoder_ThrowWhenInvalidLength()
    {
        static void TestCode() => GetDecoder(CreateMapping("RawDataDecoder(HexToAddress)", "$#3"), new JValue(RawTopicData[..(RawTopicData.Length / 2)]));

        var exception = Assert.Throws<IndexOutOfRangeException>(TestCode);
        Assert.Equal("Unable to extract chunk, index is out of line.", exception.Message);
    }

    [Fact]
    public void Decode_ValidHexString_ReturnsCorrectHexToInt32Array()
    {
        var expected = new BigInteger[] { 10000000000, 1702647882, 3406503364, 10000000000, 3406503364 };

        var converterType = new DecoderParameters("HexToBigIntegerArray");

        var converter = DataDecoderFactory.GetConverter(converterType.Decoder);
        converter.BuildFromData(HexStringToArray);
        var result = converter.DecodedData;

        ((IEnumerable<BigInteger>)result).Should().BeEquivalentTo(expected);
    }

    private static DownloaderMapping CreateMapping(string converter, string? path = null) => new()
    {
        Converter = converter,
        Name = "Value",
        Path = path ?? "$"
    };

    private static DataDecoder GetDecoder(DownloaderMapping downloaderMapping , JValue jValue)
    {
        return new DataDecoderFactory(downloaderMapping, jValue).Decoder;
    }

    [Theory]
    [InlineData(new[] { "", "https://nft.poolz.finance/bsc/metadata/" }, MainNet)]
    [InlineData(new[] { "newBase", "https://nft.poolz.finance/test/metadata/https://nft.poolz.finance/test/metadata/" }, TestNet)]
    public void Decode_ValidHex_ReturnsExpectedUrls(string[] expectedUrls, string hex )
    {
        var result = new DataDecoderFactory(CreateMapping(nameof(HexToStrings)), new JValue(hex)).Decoder;

        expectedUrls.Should().BeEquivalentTo(result.DecodedData);
    }

    [Fact]
    public void Decode_InvalidHex_ThrowsArgumentException()
    {
        var invalidHex = "0xGG";

        var result = () => new DataDecoderFactory(CreateMapping(nameof(ChunckHexToString)), new JValue(invalidHex)).Decoder;

        result.Should().Throw<FormatException > ();
    }

    [Fact]
    public void GetTokenDecimals_ShouldReturnCorrectDecimals()
    {
        var tokenAddress = new EthereumAddress("0x1234567890abcdef1234567890abcdef12345678");
        const long chainId = 1;
        const string name = "Test Token";
        const string symbol = "TTK";
        const byte decimals = 6;
        var totalSupply = new BigInteger(1000000);

        var token = new ERC20DynamoDbTable(chainId, tokenAddress, name, symbol, decimals, totalSupply);

        var mockCacheProvider = new Mock<ERC20CacheProvider>();
        mockCacheProvider.Setup(x => x.GetOrAdd(It.IsAny<GetCacheRequest>())).Returns(token);

        var decoder = new ERC20CacheManager(mockCacheProvider.Object);

        var result = decoder.GetTokenInfo(chainId, tokenAddress);

        Assert.NotNull(result);
        Assert.Equal(decimals, result.Decimals);
    }
}