using Moq;
using FluentAssertions;
using DownloaderContext;
using DownloaderV2.Result;
using DownloaderV2.Builders.DownloadHandlerBuilders;

namespace DownloaderV2.Tests
{
    public class DownloaderV2RunTests
    {
        private const string LastBlockApiKey = "covalent_key_here";
        private const string LastBlockUrlWithOutKey = "https://api.covalenthq.com/v1/chains/status/?key=";
        private const string ApiUrl = "https://api.covalenthq.com/v1/{{chainId}}/tokens/{{contractAddress}}/token_holders_v2/?page-size=100&page-number=0&key={{apiKey}}";

        public DownloaderV2RunTests()
        {
            Environment.SetEnvironmentVariable("LastBlockKey", LastBlockApiKey);
            Environment.SetEnvironmentVariable("LastBlockDownloaderUrl", LastBlockUrlWithOutKey);
            Environment.SetEnvironmentVariable("ApiUrl", ApiUrl);
        }

        [Fact]
        public async Task TestDownloaderFunctionality()
        {
            var context = await DbMock.CreateMockContextAsync();

            var settings = context.DownloaderSettings.ToList();
            var mappings = context.DownloaderMapping.ToList();

            Assert.NotEmpty(settings);
            Assert.Equal(1, settings.Count);
            Assert.Equal(13, mappings.Count);
        }

        [Fact]
        public async Task RunAsync_UsesDownloadHandler_ReturnsExpectedResults()
        {
            var mockContext = await DbMock.CreateMockContextAsync();

            var mockFactory = new Mock<IDownloadHandlerFactory>();
            var mockHandler = new Mock<IDownloadHandler>();

            var expectedResults = new List<ResultObject>
            {
                new(),
                new()
            };

            mockHandler.Setup(m => m.HandleAsync()).ReturnsAsync(expectedResults);
            mockFactory.Setup(f => f.Create(It.IsAny<BaseDownloaderContext>())).Returns(mockHandler.Object);

            var runner = new DownloaderV2Run(mockContext, mockFactory.Object);

            var results = await runner.RunAsync();

            results.Should().BeEquivalentTo(expectedResults, "because the method should return the results provided by the DownloadHandler");

            mockFactory.Verify(f => f.Create(It.Is<BaseDownloaderContext>(ctx => ctx == mockContext)), Times.Once(), "Factory should be called exactly once with the provided context");
            mockHandler.Verify(m => m.HandleAsync(), Times.Once(), "HandleAsync should be called exactly once to retrieve results");
        }
    }
}