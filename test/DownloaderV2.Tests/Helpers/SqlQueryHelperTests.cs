using Moq;
using FluentAssertions;
using DownloaderContext;
using DownloaderV2.Helpers;
using DownloaderV2.Helpers.Logger;

namespace DownloaderV2.Tests.Helpers
{
    public class SqlQueryHelperTests
    {
        private readonly BaseDownloaderContext _context;
        private readonly SqlQueryHelper _sqlQueryHelper;
        private readonly Mock<ILogger> _mockLogger;

        public SqlQueryHelperTests()
        {
            _mockLogger = new Mock<ILogger>();
            ApplicationLogger.Initialize(_mockLogger.Object);

            _context = DbMock.CreateMockContextAsync().Result;
            _sqlQueryHelper = new SqlQueryHelper(_context);
        }

        [Fact]
        public void TrySaveChangeAsync_ShouldSaveChangesSuccessfully()
        {
            var settings = _context.DownloaderSettings.FirstOrDefault(s => s.ChainId == 97);

            settings.Should().NotBeNull();
            settings!.StartingBlock = 100;

            _sqlQueryHelper.TrySaveChangeAsync();

            var updatedSettings = _context.DownloaderSettings.FirstOrDefault(s => s.ChainId == 97);
            updatedSettings.Should().NotBeNull();
            updatedSettings!.StartingBlock.Should().Be(100);
        }

        [Fact]
        public void UpdateDownloaderSettings_ShouldUpdateSettingsInDatabase()
        {
            var initialSettings = _context.DownloaderSettings.FirstOrDefault(s =>
                s.ChainId == 97 && s.ResponseType == "SwapBNBParty");

            initialSettings.Should().NotBeNull();
            var originalStartingBlock = initialSettings!.StartingBlock;
            var originalEndingBlock = initialSettings.EndingBlock;

            _sqlQueryHelper.UpdateDownloaderSettings(initialSettings, 100, 200);
            _context.SaveChanges();

            var updatedSettings = _context.DownloaderSettings.FirstOrDefault(s =>
                s.ChainId == 97 && s.ResponseType == "SwapBNBParty");

            updatedSettings.Should().NotBeNull();
            updatedSettings!.StartingBlock.Should().Be(100);
            updatedSettings.EndingBlock.Should().Be(200);

            originalStartingBlock.Should().NotBe(100);
            originalEndingBlock.Should().NotBe(200);
        }

        [Fact]
        public void TrySaveChangeAsync_ShouldLogAndThrow_InvalidOperationException()
        {
            var settings = _context.DownloaderSettings.First(s => s.ChainId == 97);

            settings.ChainId = 98;

            var act = () => _sqlQueryHelper.TrySaveChangeAsync();

            var exception = Assert.Throws<InvalidOperationException>(() => _sqlQueryHelper.TrySaveChangeAsync());
            exception.Should().NotBeNull();

            _mockLogger.Verify(logger => logger.LogCritical(It.IsAny<string>()), Times.Once);
        }
    }
}
