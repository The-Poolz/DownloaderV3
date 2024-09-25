using FluentAssertions;
using DownloaderV3.Source.CovalentDocument.Tests.Models;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument.Tests;

public class DocumentDecoderTests
{
    [Fact(Skip = "Test is not ready yet, skipping for now.")]
    public void DecodeDocument_ShouldReturnCorrectResponses_WhenInputDataIsValid()
    {
        var downloaderSettings = DownloaderSettingsMock.DownloaderSettings;

        var inputData = new InputData
        {
            Data = new Data
            {
                ChainId = 56,
                Items = new[]
                {
                    new Transaction
                    {
                        BlockHeight = 1357,
                        RawLogTopics = new[] { DownloaderSettingsMock.DownloaderSettings.EventHash, "0x123" }
                    }
                }
            }
        };

        var documentDecoder = new DocumentDecoder(downloaderSettings, inputData);

        documentDecoder.DocumentResponses.Should().NotBeNull();
        documentDecoder.DocumentResponses.Count.Should().Be(1);
    }
}