using DownloaderContext;
using DownloaderV2.Tests.Results.DbResults.CustomDownloaderContext.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DownloaderV2.Tests.Results.DbResults.CustomDownloaderContext
{
    public class CustomDownloaderContext : BaseDownloaderContext
    {
        public CustomDownloaderContext() { }
        public CustomDownloaderContext(DbContextOptions options) : base(options) { }
        public CustomDownloaderContext(DbContextOptions<CustomDownloaderContext> options) : base(options) { }

        public DbSet<SwapBNBParty> SwapBNBParty { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SwapBNBParty>(ConfigureSwapBNBParty);
        }

        private void ConfigureSwapBNBParty(EntityTypeBuilder<SwapBNBParty> builder)
        {
            builder.HasKey(e => new { e.ChainId, e.BlockHeight, e.TxHash, e.BlockSignedAt, e.SenderAddress, e.TxOffset, e.LogOffset });
        }
    }
}
