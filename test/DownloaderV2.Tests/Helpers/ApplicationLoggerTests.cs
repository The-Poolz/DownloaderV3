using Moq;
using FluentAssertions;
using DownloaderV2.Helpers.Logger;

namespace DownloaderV2.Tests.Helpers
{
    public class ApplicationLoggerTests
    {
        private readonly Mock<ILogger> _mockLogger;

        public ApplicationLoggerTests()
        {
            _mockLogger = new Mock<ILogger>();
            ApplicationLogger.Initialize(_mockLogger.Object);
        }

        [Fact]
        public void LogAndThrowDynamic_ShouldCallLoggerLogCriticalAndThrowException()
        {
            var exception = new InvalidOperationException("Test exception");
            var additionalMessage = "Additional info";

            Action act = () => ApplicationLogger.LogAndThrowDynamic(exception, additionalMessage);

            act.Should().Throw<InvalidOperationException>().WithMessage("Test exception");
            _mockLogger.Verify(logger => logger.LogCritical(It.Is<string>(msg => msg.Contains("Test exception") && msg.Contains("Additional info"))), Times.Once);
        }

        [Fact]
        public void LogAndThrow_ShouldLogAndThrowException()
        {
            var mockLogger = new Mock<ILogger>();
            ApplicationLogger.Initialize(mockLogger.Object);

            var exception = new InvalidOperationException("Test exception");

            Action act = () => ApplicationLogger.LogAndThrow(exception);

            act.Should().Throw<InvalidOperationException>().WithMessage("Test exception");

            mockLogger.Verify(l => l.LogCritical(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Log_ShouldCallLoggerLog()
        {
            var message = "This is a log message";

            ApplicationLogger.Log(message);

            _mockLogger.Verify(logger => logger.Log(It.Is<string>(msg => msg == message)), Times.Once);
        }
    }
}