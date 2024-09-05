using DownloaderV2.Builders.LastBlockBuilder;
using DownloaderV2.Builders.LastBlockBuilder.SourcePage;
using DownloaderV2.Models.LastBlock;
using Moq;
using Newtonsoft.Json;

namespace DownloaderV2.Tests.Builders.LastBlockBuilder
{
    public class LastBlockDownloaderTest
    {
        [Fact]
        public async Task LastBlockDictionary_ReturnsCorrectData()
        {
            var mockLastBlockService = new Mock<GetSourcePage>();
            var expectedDictionary = new Dictionary<long, long>
            {
                { 1, 100 },
                { 2, 200 }
            };

            mockLastBlockService.Setup(service => service.FetchDataAsync())
                .ReturnsAsync(expectedDictionary);

            var downloader = new LastBlockDownloader(mockLastBlockService.Object);

            var actualDictionary = await downloader.LastBlockDictionary;

            Assert.NotNull(actualDictionary);
            Assert.Equal(expectedDictionary, actualDictionary);
        }
    }
}
