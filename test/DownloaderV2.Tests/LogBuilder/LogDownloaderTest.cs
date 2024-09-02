using Flurl.Http.Testing;
using DownloaderContext.Models;
using DownloaderV2.Builders.LogBuilder;
using DownloaderV2.Models.Covalent;
using DownloaderV2.Tests.Results.CovalentResults;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;

namespace DownloaderV2.Tests.LogBuilder;

public class LogDownloaderTest
{
    /*[Fact]
    public void LogDownloader_ShouldFormCorrectUrl()
    {
        using var httpTest = new HttpTest();

        var downloaderSettings = DbMock.CreateMockContextAsync().Result;
        var downloaderSetting = downloaderSettings.DownloaderSettings.First();

        var lastBlockDictionary = new Dictionary<long, long> { { 97, 1500 } };
        var chainSettings = new Dictionary<long, ChainInfo>
        {
            { 97, new ChainInfo { BlockPerSecond = 1, DownloadTimeDelay = 500 } }
        };

        const string expectedUrl = "https://api.example.com/v1/1/events/0xContractAddress/?starting-block=100&ending-block=1500&page-number=1&key=apiKey";

        httpTest.RespondWithJson(JObject.Parse(CovalentResultConst.SwapBNBPartyString));

        var logDownloader = new LogDownloader(1, downloaderSetting, lastBlockDictionary, chainSettings);
        var actualUrl = logDownloader.Url.ParseUrl(downloaderSetting.UrlSet);

        actualUrl.Should().Be(expectedUrl);
        logDownloader.DownloadedContractData.Should().NotBeNull();
        logDownloader.DownloadedContractData.Should().BeOfType<InputData>();

        httpTest.ShouldHaveCalled(expectedUrl)
            .WithVerb(HttpMethod.Get)
            .Times(1);
    }*/
}