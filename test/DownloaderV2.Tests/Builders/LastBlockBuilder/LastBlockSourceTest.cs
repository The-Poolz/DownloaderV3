using Moq;
using FluentAssertions;
using SourceLastBlock.AbstractClass;
using DownloaderV2.Builders.LastBlockBuilder;

namespace DownloaderV2.Tests.Builders.LastBlockBuilder
{
    public class LastBlockSourceTest
    {
        public LastBlockSourceTest()
        {
            Environment.SetEnvironmentVariable("LastBlockDownloaderUrl", "https://api?");
            Environment.SetEnvironmentVariable("LastBlockKey", "key");
        }

        [Fact]
        public async Task LastBlockDictionary_ShouldReturnCorrectData()
        {
            var mockGetSourcePage = new Mock<GetSourcePage>();

            var expectedDictionary = new Dictionary<long, long>
            {
                { 1, 100 },
                { 2, 200 }
            };

            mockGetSourcePage.Setup(page => page.FetchDataAsync()).ReturnsAsync(expectedDictionary);

            LastBlockSource.Initialize(mockGetSourcePage.Object);
            var result = await LastBlockSource.Instance.LastBlockDictionary;

            result.Should().BeEquivalentTo(expectedDictionary);
        }

        [Fact]
        public void Instance_ShouldThrowException_WhenNotInitialized()
        {
            var act = () => { var instance = LastBlockSource.Instance; };

            act.Should().Throw<InvalidOperationException>().WithMessage("LastBlockSource not initialized.");
        }
    }
}
