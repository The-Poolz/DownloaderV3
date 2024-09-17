using FluentAssertions;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Decoders;
using DownloaderV3.Source.CovalentDocument.Tests.Models;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument.Tests.Decoders;

public class TopicDecoderTests
{
    [Fact]
    public void TopicDecoder_ShouldInitializeLogDataCorrectly()
    {
        var original = new Data
        {
            ChainId = 1,
            UpdatedAt = DateTime.UtcNow,
            Items = new Transaction[]
            {
                new Transaction
                {
                    BlockHeight = 26993304
                }
            },
            Pagination = new Pagination { HasMore = true, PageNumber = 1 }
        };

        var topicDecoder = new TopicDecoder(original, DownloaderSettingsMock.DownloaderMappings);

        topicDecoder.LogData.Should().ContainKey("ChainId");
        topicDecoder.LogData.Should().ContainKey("BlockHeight");

        var chainIdDecoder = topicDecoder.LogData["ChainId"];
        var updatedAtDecoder = topicDecoder.LogData["BlockHeight"];

        chainIdDecoder.Should().NotBeNull();
        updatedAtDecoder.Should().NotBeNull();
    }
}