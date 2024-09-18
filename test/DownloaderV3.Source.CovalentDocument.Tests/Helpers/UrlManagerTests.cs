using FluentAssertions;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Helpers;

namespace DownloaderV3.Source.CovalentDocument.Tests.Helpers
{
    public class UrlManagerTests
    {
        [Fact]
        public void SetupUrlParser_ShouldGenerateCorrectUrl()
        {
            var downloaderSettings = new DownloaderSettings
            {
                ChainId = 1,
                ContractAddress = "0x1234567890abcdef",
                StartingBlock = 1000,
                UrlSet = "https://api.covalenthq.com/v1/{{ChainId}}/events/address/{{ContractAddress}}/?starting-block={{StartingBlock}}&ending-block={{EndingBlock}}&page-number={{PageNumber}}&page-size={{MaxPageNumber}}&key={{Key}}",
                MaxPageNumber = 100,
                Key = "my-api-key"
            };

            const int pageNumber = 1;
            const int savedLastBlock = 2000;

            var result = UrlManager.SetupUrlParser(pageNumber, downloaderSettings, savedLastBlock);

            result.Should().Be("https://api.covalenthq.com/v1/1/events/address/0x1234567890abcdef/?starting-block=1000&ending-block=2000&page-number=1&page-size=100&key=my-api-key");
        }
    }
}