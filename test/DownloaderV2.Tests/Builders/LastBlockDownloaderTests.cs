using FluentAssertions;
using Flurl.Http.Testing;
using Newtonsoft.Json.Linq;
using DownloaderV2.Builders;
using DownloaderV2.Builders.LastBlockBuilder;
using DownloaderV2.Tests.Results.CovalentResults;

namespace DownloaderV2.Tests.Builders
{
    public class LastBlockDownloaderTests
    {
        private const string LastBlockApiKey = "covalent_key_here";
        private const string LastBlockUrlWithOutKey = "https://api.covalenthq.com/v1/chains/status/?key=";
        private const string ApiUrl = "https://api.covalenthq.com/v1/{{chainId}}/tokens/{{contractAddress}}/token_holders_v2/?page-size=100&page-number=0&key={{apiKey}}";

        public LastBlockDownloaderTests()
        {
            Environment.SetEnvironmentVariable("LastBlockKey", LastBlockApiKey);
            Environment.SetEnvironmentVariable("LastBlockDownloaderUrl", LastBlockUrlWithOutKey);
            Environment.SetEnvironmentVariable("ApiUrl", ApiUrl);
        }

        private class TestDownloader : BaseDownloader
        {
            public JToken? GetDownloadData() => DownloadData;
        }

        [Fact]
        public void BaseDownloader_ShouldReturnValidDownloadData()
        {
            using var httpTest = new HttpTest();

            httpTest.RespondWith(CovalentResultConst.LastBlockString);
            var downloader = new TestDownloader();

            var downloadData = downloader.GetDownloadData();

            downloadData.Should().NotBeNull();

            var items = downloadData!["data"]?["items"];
            items.Should().NotBeNull();
            items!.Should().HaveCount(4);

            var expectedValues = new (int ChainId, int BlockHeight)[]
            {
                (1, 17165351),
                (56, 27826734),
                (42161, 86156198),
                (97, 27826734)
            };

            for (var i = 0; i < expectedValues.Length; i++)
            {
                var item = items![i];
                item.Should().NotBeNull();
                item!["chain_id"]!.Value<int>().Should().Be(expectedValues[i].ChainId);
                item["synced_block_height"]!.Value<int>().Should().Be(expectedValues[i].BlockHeight);
            }
        }

        [Fact]
        public void LastBlockDownloader_ShouldFillLastBlockDictionaryCorrectly()
        {
            using var httpTest = new HttpTest();

            httpTest.RespondWith(CovalentResultConst.LastBlockString);
            var downloader = new LastBlockDownloader();

            var lastBlockDictionary = downloader.LastBlockDictionary;

            lastBlockDictionary.Should().HaveCount(4);
            lastBlockDictionary[1].Should().Be(17165351);
            lastBlockDictionary[56].Should().Be(27826734);
            lastBlockDictionary[42161].Should().Be(86156198);
            lastBlockDictionary[97].Should().Be(27826734);
        }

        [Fact]
        public void BaseDownloader_ShouldBuildCorrectUri()
        {
            var expectedUrl = $"{LastBlockUrlWithOutKey}{LastBlockApiKey}";

            var downloader = new BaseDownloader();
            var actualUrl = downloader.GetUri;

            actualUrl.Should().Be(expectedUrl);
        }
    }
}