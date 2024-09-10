using Moq;
using FluentAssertions;
using SourceLastBlock.SourcePage;
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
        public void LastBlockDictionary_ShouldReturnCorrectData()
        {
            var mockGetSourcePage = new Mock<GetSourcePage>();

            var expectedDictionary = new Dictionary<long, long>
            {
                { 1, 100 },
                { 2, 200 }
            };

            mockGetSourcePage.Setup(page => page.FetchData()).Returns(expectedDictionary);

            var result = new LastBlockSource(mockGetSourcePage.Object).LastBlockDictionary;

            result.Should().BeEquivalentTo(expectedDictionary);
        }
    }
}
