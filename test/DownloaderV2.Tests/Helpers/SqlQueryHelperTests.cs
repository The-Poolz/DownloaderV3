using FluentAssertions;
using DownloaderContext;
using DownloaderV2.Helpers;

namespace DownloaderV2.Tests.Helpers
{
    public class SqlQueryHelperTests
    {
        private readonly BaseDownloaderContext _context;
        private readonly SqlQueryHelper _sqlQueryHelper;

        public SqlQueryHelperTests()
        {
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
    }
}
