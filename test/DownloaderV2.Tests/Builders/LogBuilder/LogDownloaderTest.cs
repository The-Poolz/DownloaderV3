using FluentAssertions;
using Flurl.Http.Testing;
using DownloaderContext.Models;
using DownloaderV2.Models.Covalent;
using DownloaderV2.Builders.LogBuilder;
using DownloaderV2.Tests.Results.CovalentResults;

namespace DownloaderV2.Tests.Builders.LogBuilder;

public class LogDownloaderTest
{
    [Fact]
    public void LogDownloader_ShouldFormCorrectUrl()
    {
        using var httpTest = new HttpTest();

        var downloaderSettings = DbMock.CreateMockContextAsync().Result;
        var downloaderSetting = downloaderSettings.DownloaderSettings.First();

        var lastBlockDictionary = new Dictionary<long, long> { { 97, 1000 } };
        var chainSettings = new Dictionary<long, ChainInfo>
        {
            { 97, new ChainInfo { BlockPerSecond = 1, DownloadTimeDelay = 500 } }
        };

        const string expectedUrl = "https://api.covalenthq.com/v1/97/events/address/0x1Fa306AAfcbf6B5e13a19c02E044ee28588B48ae/?starting-block=1&ending-block=500&page-number=1&page-size=1000&key=myKey";

        httpTest.RespondWith(CovalentResultConst.SwapBNBPartyString);

        var logDownloader = new LogDownloader(1, downloaderSetting, lastBlockDictionary, chainSettings);
        var actualUrl = logDownloader.Url.ParseUrl(downloaderSetting.UrlSet);

        var areEqual = string.Equals(expectedUrl, actualUrl, StringComparison.Ordinal);
        areEqual.Should().BeTrue("Both URLs should be exactly the same");
        logDownloader.DownloadedContractData.Should().NotBeNull();
        logDownloader.DownloadedContractData.Should().BeOfType<InputData>();

        httpTest.ShouldHaveCalled(expectedUrl)
            .WithVerb(HttpMethod.Get)
            .Times(1);
    }
}