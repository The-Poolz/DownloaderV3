using FluentAssertions;
using DownloaderV2.Tests.Results;
using DownloaderV2.Tests.Results.DbResults;

namespace DownloaderV2.Tests
{
    public class DownloadHandlerTests
    {
        private const string LastBlockApiKey = "covalent_key_here";
        private const string LastBlockUrlWithOutKey = "https://api.covalenthq.com/v1/chains/status/?key=";
        private const string ApiUrl = "https://api.covalenthq.com/v1/{{chainId}}/tokens/{{contractAddress}}/token_holders_v2/?page-size=100&page-number=0&key={{apiKey}}";

        private readonly DownloadHandler _downloadHandler;

        public DownloadHandlerTests()
        {
            Environment.SetEnvironmentVariable("LastBlockKey", LastBlockApiKey);
            Environment.SetEnvironmentVariable("LastBlockDownloaderUrl", LastBlockUrlWithOutKey);
            Environment.SetEnvironmentVariable("ApiUrl", ApiUrl);

            var mockContext = DbMock.CreateMockContextAsync().Result;

            _downloadHandler = new DownloadHandler(mockContext);
        }

        [Fact]
        public void AddResult_ShouldStoreCorrectDataInResultBuilder()
        {
            var mockSettings = DownloaderSettingsResult.SwapBNBParty1;
            const int eventCount = 10;

            _downloadHandler.AddResult(mockSettings, eventCount);

            var result = _downloadHandler.ResultBuilder.Result;
            var expectedResult = ResultBuilderMock.ResultList[0];

            result.Should().ContainSingle(ro =>
                ro.ChainId == expectedResult.ChainId &&
                ro.EventName == expectedResult.EventName &&
                ro.Count == expectedResult.Count
            );
        }
    }
}
