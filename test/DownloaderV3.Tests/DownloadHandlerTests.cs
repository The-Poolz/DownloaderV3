using Moq;
using FluentAssertions;
using Moq.EntityFrameworkCore;
using DownloaderV3.Destination;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentLastBlock.SourcePage;
using DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Tests;

public class DownloadHandlerTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;

    public DownloadHandlerTests()
    {
        Environment.SetEnvironmentVariable("LastBlockDownloaderUrl", "https://api?");
        Environment.SetEnvironmentVariable("LastBlockKey", "key");
        Environment.SetEnvironmentVariable("AWS_REGION", "us-east-1");
        Environment.SetEnvironmentVariable("ApiUrl", "test");

        _serviceProviderMock = new Mock<IServiceProvider>();
        Mock<IDocumentDecoderFactory> documentDecoderFactoryMock = new();
        Mock<IDocumentFactory> documentFactoryMock = new();
        Mock<BaseDestination> destinationMock = new();
        Mock<GetSourcePage> getSourcePageMock = new();

        var mockDownloaderSettings = new List<DownloaderSettings> { new DownloaderSettings { Active = true } };

        var mockChainInfoData = new List<ChainInfo> { new ChainInfo { ChainId = 1 } };

        destinationMock.Setup(d => d.DownloaderSettings).ReturnsDbSet(mockDownloaderSettings);
        destinationMock.Setup(d => d.ChainsInfo).ReturnsDbSet(mockChainInfoData);

        _serviceProviderMock.Setup(sp => sp.GetService(typeof(IDocumentDecoderFactory)))
            .Returns(documentDecoderFactoryMock.Object);

        _serviceProviderMock.Setup(sp => sp.GetService(typeof(IDocumentFactory))).Returns(documentFactoryMock.Object);

        _serviceProviderMock.Setup(sp => sp.GetService(typeof(BaseDestination)))
            .Returns(destinationMock.Object);
        _serviceProviderMock.Setup(sp => sp.GetService(typeof(GetSourcePage)))
            .Returns(getSourcePageMock.Object);
    }

    [Fact]
    public void Constructor_ShouldResolveDependenciesCorrectly()
    {
        var handler = new DownloadHandler<InputData>(_serviceProviderMock.Object);

        _serviceProviderMock.Verify(sp => sp.GetService(typeof(BaseDestination)), Times.Once);
        _serviceProviderMock.Verify(sp => sp.GetService(typeof(GetSourcePage)), Times.Once);
        _serviceProviderMock.Verify(sp => sp.GetService(typeof(IDocumentDecoderFactory)), Times.Once);
        _serviceProviderMock.Verify(sp => sp.GetService(typeof(IDocumentFactory)), Times.Once);

        handler.Should().NotBeNull();
    }
}