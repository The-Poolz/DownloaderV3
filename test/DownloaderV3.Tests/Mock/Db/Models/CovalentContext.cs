using DownloaderV3.Destination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DownloaderV3.Tests.Mock.Db.Models;

public class CovalentContext : BaseDestination
{
    public CovalentContext() { }
    public CovalentContext(DbContextOptions options) : base(options) { }

    public virtual DbSet<SwapParty> SwapParty { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SwapParty>(ConfigureSwapParty);
    }

    private void ConfigureSwapParty(EntityTypeBuilder<SwapParty> builder)
    {
        builder.HasKey(e => new { e.ChainId, e.BlockHeight, e.TxHash, e.BlockSignedAt, e.SenderAddress, e.TxOffset, e.LogOffset });
    }
}