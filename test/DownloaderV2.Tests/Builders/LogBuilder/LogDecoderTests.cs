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
#pragma warning disable xUnit1031
            var context = DbMock.CreateMockContextAsync().Result;
#pragma warning restore xUnit1031
            LogRouter.LogRouter.Initialize(context.GetType());

            var downloaderSettings = context.DownloaderSettings.First(s =>
                s.ResponseType == "SwapBNBParty");

            var inputData = JsonConvert.DeserializeObject<InputData>(CovalentResultConst.SwapBNBPartyString);

            
            var logDecoder = new LogDecoder(downloaderSettings, inputData!);

            logDecoder.LogResponses.Should().NotBeNull();
            logDecoder.EventCount.Should().Be(1);

            var decodedData = logDecoder.LogResponses.FirstOrDefault();
            decodedData.Should().NotBeNull();
        }
    }
}
