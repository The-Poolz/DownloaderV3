using DownloaderContext;
using DownloaderContext.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DownloaderV2.Tests
{
    public class DownloaderV2RunTests
    {
        private readonly Mock<BaseDownloaderContext> _mockContext;

        public DownloaderV2RunTests()
        {
            _mockContext = new Mock<BaseDownloaderContext>();

            var settings = new List<DownloaderSettings>
            {
                new DownloaderSettings
                {
                    ChainId = 97,
                    StartingBlock = 1,
                    EndingBlock = 100,
                    MaxBatchSize = 50000,
                    Key = "myKey",
                    ResponseType = "SwapBNBParty",
                    ContractAddress = "0x1Fa306AAfcbf6B5e13a19c02E044ee28588B48ae",
                    EventHash = "0xc42079f94a6350d7e6235f29174924f928cc2ac818eb64fed8004e115fbcca67",
                    Active = true,
                    MaxPageNumber = 1000,
                    UrlSet = "https://api.covalenthq.com/v1/[ChainId]/events/address/[ContractAddress]/?starting-block=[StartingBlock]&ending-block=[EndingBlock]&page-number=[PageNumber]&page-size=[MaxPageNumber]&key=[Key]"

                }
            };

            var settingsMockSet = CreateMockDbSet(settings);
            _mockContext.Setup(m => m.DownloaderSettings).Returns(settingsMockSet.Object);

            var mappings = new List<DownloaderMapping>
            {
                new DownloaderMapping { Name = "ChainId", Converter = "StringToInt32", Path = "$.chain_id" },
                new DownloaderMapping { Name = "BlockHeight", Converter = "StringToInt32", Path = "$.Items[0].Block_height" },
                new DownloaderMapping { Name = "TxHash", Converter = "HexToString", Path = "$.Items[0].Tx_hash" }

            };

            var mappingsMockSet = CreateMockDbSet(mappings);
            _mockContext.Setup(m => m.DownloaderMapping).Returns(mappingsMockSet.Object);
        }

        private static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            using var enumerator = queryable.GetEnumerator();
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(enumerator);
            return dbSet;
        }

        [Fact]
        public void TestDownloaderFunctionality()
        {
            var settings = _mockContext.Object.DownloaderSettings.ToList();
            var mappings = _mockContext.Object.DownloaderMapping.ToList();

            settings.Should().NotBeEmpty();
            mappings.Should().HaveCount(3);
        }
    }
}