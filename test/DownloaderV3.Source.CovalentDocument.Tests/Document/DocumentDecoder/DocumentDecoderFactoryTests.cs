using Moq;
using FluentAssertions;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;
using DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;

namespace DownloaderV3.Source.CovalentDocument.Tests.Document.DocumentDecoder;

public class DocumentDecoderFactoryTests
{
    [Fact]
    public void DocumentDecoderFactory_ShouldCreateDocumentDecoder_WhenInputDataIsProvided()
    {
        var settings = new DownloaderSettings();
        var inputData = new InputData
        {
            Data = new Data
            {
                Items = new Transaction[]
                {
                    new Transaction
                    {
                        RawLogTopics = new string[] { "0x0", "0x1" }
                    }
                },
                Pagination = new Pagination { HasMore = false }
            }
        };
        var factoryMock = new Mock<IDocumentDecoderFactory>();

        factoryMock
            .Setup(f => f.Create(settings, inputData))
            .Returns(new Source.CovalentDocument.DocumentDecoder(settings, inputData));

        var decoder = factoryMock.Object.Create(settings, inputData);

        decoder.Should().NotBeNull();
        decoder.Should().BeOfType<Source.CovalentDocument.DocumentDecoder>();
    }

    [Fact]
    public void DocumentDecoderFactory_ShouldThrowNotSupportedException_WhenInvalidDataIsProvided()
    {
        var settings = new DownloaderSettings();
        var invalidData = new object();
        var factory = new DocumentDecoderFactory();

        Action action = () => factory.Create(settings, invalidData);

        action.Should().Throw<NotSupportedException>()
            .WithMessage($"Decoder for type {invalidData.GetType().Name} is not supported.");
    }
}