using Newtonsoft.Json;
using FluentAssertions;
using Flurl.Http.Testing;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Tests.Models;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;
using DownloaderV3.Source.CovalentDocument.Tests.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument.Tests
{
    public class CovalentDocumentTests
    {
        [Fact]
        public void CovalentDocument_ShouldFetchDataAndDeserializeCorrectly()
        {
            using var httpTest = new HttpTest();
            httpTest.RespondWithJson(JsonConvert.DeserializeObject<InputData>(CovalentResultConst.EventString));

            var downloaderSettings = DownloaderSettingsMock.DownloaderSettings;
            var lastBlockDictionary = new Dictionary<long, long> { { 56, 200 } };
            var chainSettings = new Dictionary<long, ChainInfo> { { 56, new ChainInfo { BlockPerSecond = 10, DownloadTimeDelay = 5 } } };

            var document = new CovalentDocument(0, downloaderSettings, lastBlockDictionary, chainSettings);

            document.DownloadedContractData.Should().NotBeNull();
            document.SavedLastBlock.Should().Be(150);
            document.SourceLastBlock.Should().Be(200);
            document.Url.Should().Be("https://api/56/events/address/0x1Fa306AAfcbf6B5e13a19c02E044ee28588B0000/?starting-block=1&ending-block=150&page-number=0&page-size=1000&key=myKey");
        }

        [Fact]
        public void CovalentDocument_ShouldThrowException_WhenStartingBlockIsInvalid()
        {
            using var httpTest = new HttpTest();

            httpTest.RespondWithJson(JsonConvert.DeserializeObject<InputData>(CovalentResultConst.EventString));
            var downloaderSettings = DownloaderSettingsMock.DownloaderSettings;
            var lastBlockDictionary = new Dictionary<long, long> { { 56, 1 } }; 
            var chainSettings = new Dictionary<long, ChainInfo> { { 56, new ChainInfo { BlockPerSecond = 10, DownloadTimeDelay = 5 } } };

            var act = () =>
            {
                var covalentDocument = new CovalentDocument(0, downloaderSettings, lastBlockDictionary, chainSettings);
            };

            act.Should().Throw<InvalidOperationException>().WithMessage("Invalid starting block: 1");
        }

        [Fact]
        public void GetResponse_ShouldReturnValidResponse()
        {
            using var httpTest = new HttpTest();

            httpTest.RespondWithJson(JsonConvert.DeserializeObject<InputData>(CovalentResultConst.EmptyString));
            var downloaderSettings = DownloaderSettingsMock.DownloaderSettings;
            var lastBlockDictionary = new Dictionary<long, long> { { 56, 100 } };
            var chainSettings = new Dictionary<long, ChainInfo> { { 56, new ChainInfo { BlockPerSecond = 10, DownloadTimeDelay = 5 } } };

            var document = new CovalentDocument(0, downloaderSettings, lastBlockDictionary, chainSettings);

            var response = document.GetResponse();

            response.Should().Be("{\"Data\":{\"ChainId\":56,\"UpdatedAt\":\"2023-10-25T12:11:54.8050519Z\",\"Items\":[],\"Pagination\":{\"HasMore\":false,\"PageNumber\":0,\"PageSize\":100,\"TotalCount\":null}},\"Error\":false,\"ErrorMessage\":null,\"ErrorCode\":null}");
        }
    }
}