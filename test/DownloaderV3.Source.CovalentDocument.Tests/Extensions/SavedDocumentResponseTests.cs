using Moq;
using FluentAssertions;
using DownloaderV3.Destination;
using DownloaderV3.Source.CovalentDocument.Extensions;
using DownloaderV3.Source.CovalentDocument.DocumentRouter;

namespace DownloaderV3.Source.CovalentDocument.Tests.Extensions
{
    public class SavedDocumentResponseTests
    {
        [Fact]
        public void LockedSaveAll_ShouldCallSaveOnAllResponses()
        {
            var destinationMock = new Mock<BaseDestination>();

            var responseMock1 = new Mock<IDocumentResponse>();
            var responseMock2 = new Mock<IDocumentResponse>();

            var savedDocumentResponse = new SavedDocumentResponse
            {
                responseMock1.Object,
                responseMock2.Object
            };

            savedDocumentResponse.LockedSaveAll(destinationMock.Object);

            responseMock1.Verify(r => r.Save(It.IsAny<BaseDestination>()), Times.Once);
            responseMock2.Verify(r => r.Save(It.IsAny<BaseDestination>()), Times.Once);
        }

        [Fact]
        public void LockedSaveAll_ShouldLockOnDestination()
        {
            var destinationMock = new Mock<BaseDestination>();
            var responseMock = new Mock<IDocumentResponse>();

            var lockAcquired = false;
            var lockObject = destinationMock.Object;

            responseMock.Setup(r => r.Save(It.IsAny<BaseDestination>())).Callback(() =>
            {
                lock (lockObject)
                {
                    lockAcquired = true;
                }
            });

            var savedDocumentResponse = new SavedDocumentResponse
            {
                responseMock.Object
            };

            savedDocumentResponse.LockedSaveAll(destinationMock.Object);

            lockAcquired.Should().BeTrue("LockedSaveAll should acquire lock on the destination object.");
        }

        [Fact]
        public void LockedSaveAll_ShouldNotThrow_WhenNoResponsesExist()
        {
            var destinationMock = new Mock<BaseDestination>();

            var savedDocumentResponse = new SavedDocumentResponse();

            var exception = Record.Exception(() => savedDocumentResponse.LockedSaveAll(destinationMock.Object));

            exception.Should().BeNull("LockedSaveAll should not throw an exception when there are no responses.");
        }
    }
}