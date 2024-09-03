using Newtonsoft.Json;
using FluentAssertions;
using DownloaderV2.Models.Covalent;
using DownloaderV2.Builders.LogBuilder;
using DownloaderV2.Tests.Results.CovalentResults;

namespace DownloaderV2.Tests.Builders.LogBuilder
{
    public class LogDecoderTests
    {
        [Fact]
        public void LogDecoder_ShouldDecodeLogsCorrectly_WithRealData()
        {
            var context = DbMock.CreateMockContextAsync().Result;
            LogRouter.LogRouter.Initialize(context.GetType());

            var downloaderSettings = DbMock.CreateMockContextAsync().Result.DownloaderSettings.First(s =>
                s.ResponseType == "SwapBNBParty");

            var inputData = JsonConvert.DeserializeObject<InputData>(CovalentResultConst.SwapBNBPartyString);

            
            var logDecoder = new LogDecoder(downloaderSettings, inputData);

            logDecoder.LogResponses.Should().NotBeNull();
            logDecoder.EventCount.Should().Be(1);

            var decodedData = logDecoder.LogResponses.FirstOrDefault();
            decodedData.Should().NotBeNull();
        }
    }
}
