using FluentAssertions;
using DownloaderV2.Result;
using DownloaderContext.Models;

namespace DownloaderV2.Tests.Results
{
    public class ResultBuilderTests
    {
        [Fact]
        public void CreateResultObject_ShouldSetCorrectValues()
        {
            var settings = new DownloaderSettings
            {
                ChainId = 1,
                ResponseType = "SomeEvent"
            };
            var eventCount = 5;

            var resultObject = new ResultObject().SetSuccess(settings, eventCount);

            resultObject.ChainId.Should().Be(1);
            resultObject.EventName.Should().Be("SomeEvent");
            resultObject.Count.Should().Be(5);
        }

        [Fact]
        public void AddResultToBuilder_ShouldStoreCorrectly()
        {
            var resultBuilder = new ResultBuilder();

            var resultObject1 = ResultBuilderMock.ResultList[0];
            var resultObject2 = ResultBuilderMock.ResultList[1];

            resultBuilder.AddResult(resultObject1);
            resultBuilder.AddResult(resultObject2);

            resultBuilder.Result.Should().Contain(resultObject1);
            resultBuilder.Result.Should().Contain(resultObject2);
        }

        [Fact]
        public void ToString_ShouldReturnCorrectString_WhenCountGreaterThanZero()
        {
            var resultBuilder = new ResultBuilder();

            var resultObject1 = ResultBuilderMock.ResultList[0];
            var resultObject2 = ResultBuilderMock.ResultList[1];

            resultBuilder.AddResult(resultObject1);
            resultBuilder.AddResult(resultObject2);

            var resultString = resultBuilder.ToString();

            resultString.Should().Contain("| 10 saved | ChainID     97 | SomeEvent\n");
            resultString.Should().Contain("| 1 saved | ChainID     97 | SomeAnotherEvent\n");
        }
    }
}