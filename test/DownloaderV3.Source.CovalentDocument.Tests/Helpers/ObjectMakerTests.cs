using FluentAssertions;
using Newtonsoft.Json.Linq;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Helpers;
using DownloaderV3.Source.CovalentDocument.Decoders;
using DownloaderV3.Source.CovalentDocument.Decoders.DataDecoders;

namespace DownloaderV3.Source.CovalentDocument.Tests.Helpers
{
    public class ObjectMakerTests
    {
        private class TestClass
        {
            public int ChainId { get; set; }
            public long BlockHeight { get; set; }
            public bool IsValid { get; set; }
        }

        private static DownloaderMapping CreateMapping(string converter, string? path = null) => new()
        {
            Converter = converter,
            Name = "Value",
            Path = path ?? "$"
        };

        [Fact]
        public void MakeObject_ShouldSetPropertiesBasedOnDataList()
        {
            var testObject = new TestClass();

            var chainId = new DataDecoderFactory(CreateMapping(nameof(StringToInt32)), new JValue("56")).Decoder;
            var blockHeight = new DataDecoderFactory(CreateMapping(nameof(HexToLong)), new JValue("0000000000000000000000000000000000000000000000007fffffffffffffff")).Decoder;
            var hexToBool = new DataDecoderFactory(CreateMapping(nameof(HexToBool)), new JValue("0000000000000000000000000000000000000000000000000000000000000001")).Decoder;

            var dataList = new Dictionary<string, DataDecoder>
            {
                { "ChainId", chainId },
                { "BlockHeight", blockHeight },
                { "IsValid", hexToBool }
            };

            ObjectMaker.MakeObject(testObject, dataList);

            testObject.ChainId.Should().Be(56);
            testObject.BlockHeight.Should().Be(long.MaxValue);
            testObject.IsValid.Should().BeTrue();
        }

        [Fact]
        public void MakeObject_ShouldIgnoreNonMatchingProperties()
        {
            var testObject = new TestClass();

            var responseType = new DataDecoderFactory(CreateMapping(nameof(HexToString)), new JValue("PoolDeactivated")).Decoder;

            var dataList = new Dictionary<string, DataDecoder>
            {
                { "NonExistentProperty", responseType }
            };

            ObjectMaker.MakeObject(testObject, dataList);

            testObject.ChainId.Should().Be(0);
            testObject.BlockHeight.Should().Be(0);
            testObject.IsValid.Should().BeFalse();
        }
    }
}
