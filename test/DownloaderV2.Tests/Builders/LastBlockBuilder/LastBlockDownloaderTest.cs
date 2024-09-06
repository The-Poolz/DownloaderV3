using Moq;
using DownloaderV2.Builders.LastBlockBuilder;
using DownloaderV2.Builders.LastBlockBuilder.SourcePage;

namespace DownloaderV2.Tests.Builders.LastBlockBuilder
{
    public class LastBlockDownloaderTest
    {
        public LastBlockDownloaderTest()
        {
            Environment.SetEnvironmentVariable("LastBlockDownloaderUrl", "https://api?");
            Environment.SetEnvironmentVariable("LastBlockKey", "key");
        }

        [Fact]
        public async Task LastBlockDictionary_ReturnsCorrectData()
        {
            var mockLastBlockService = new Mock<GetLastBlock>();
            var expectedDictionary = new Dictionary<long, long>
            {
                { 1, 100 },
                { 2, 200 }
            };

            mockLastBlockService.Setup(service => service.FetchDataAsync())
                .ReturnsAsync(expectedDictionary);

            ResetSingleton();

            var downloader = LastBlockSource.GetInstance(mockLastBlockService.Object);
            var actualDictionary = await downloader.LastBlockDictionary;

            Assert.NotNull(actualDictionary);
            Assert.Equal(expectedDictionary, actualDictionary);
        }

        [Fact]
        public void GetInstance_ShouldReturnSameInstance()
        {
            var mockGetSourcePage = new Mock<GetLastBlock>("testUri");

            var instance1 = LastBlockSource.GetInstance(mockGetSourcePage.Object);
            var instance2 = LastBlockSource.GetInstance(mockGetSourcePage.Object);

            Assert.Equal(instance1, instance2);
        }

        private static void ResetSingleton()
        {
            typeof(LastBlockSource)
                .GetField("_instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, null);
        }
    }
}
