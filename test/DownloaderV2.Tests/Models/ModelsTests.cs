using Newtonsoft.Json;
using FluentAssertions;
using DownloaderV2.Models.LastBlock;
using DownloaderV2.Tests.Results.CovalentResults;

namespace DownloaderV2.Tests.Models
{
    public class ModelsTests
    {
        [Fact]
        public void LastBlockResponse_ShouldDeserializeCorrectly()
        {
            var response = JsonConvert.DeserializeObject<LastBlockResponse>(CovalentResultConst.LastBlockString);

            response.Should().NotBeNull();
            response!.Data.Should().NotBeNull();
            response.Data.Items.Should().HaveCount(4);

            var expectedValues = new (long ChainId, long BlockHeight, string Name, bool IsTestnet)[]
            {
                (1, 17165351, "eth-mainnet", false),
                (56, 27826734, "bsc-mainnet", false),
                (42161, 86156198, "arbitrum-mainnet", false),
                (97, 27826734, "bnb-testnet", true)
            };

            for (int i = 0; i < expectedValues.Length; i++)
            {
                var item = response.Data.Items[i];
                item.ChainId.Should().Be(expectedValues[i].ChainId);
                item.BlockHeight.Should().Be(expectedValues[i].BlockHeight);
                item.Name.Should().Be(expectedValues[i].Name);
                item.IsTestnet.Should().Be(expectedValues[i].IsTestnet);
            }
        }
    }
}