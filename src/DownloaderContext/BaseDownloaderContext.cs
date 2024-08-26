using DownloaderContext.Types;
using DownloaderContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DownloaderContext
{
    public class BaseDownloaderContext : DbContext
    {
        public BaseDownloaderContext() { }
        public BaseDownloaderContext(DbContextOptions options) : base(options) { }
        public BaseDownloaderContext(DbContextOptions<BaseDownloaderContext> options) : base(options) { }

        public virtual DbSet<Chain> Chains { get; set; } = null!;
        public virtual DbSet<ChainInfo> ChainsInfo { get; set; } = null!;
        public virtual DbSet<DownloaderSettings> DownloaderSettings { get; set; } = null!;
        public virtual DbSet<DownloaderMapping> DownloaderMapping { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chain>(ConfigureChain);
            modelBuilder.Entity<ChainInfo>(ConfigureChainInfo);
            modelBuilder.Entity<DownloaderSettings>(ConfigureDownloaderSettings);
            modelBuilder.Entity<DownloaderMapping>(ConfigureDownloaderMapping);
        }

        private void ConfigureChain(EntityTypeBuilder<Chain> builder)
        {
            builder.HasKey(x => x.ChainId);
            builder.Property(e => e.ChainId).ValueGeneratedNever();
            builder.HasIndex(x => x.ChainId)
                .IsUnique();

            builder.Property(e => e.Name).HasColumnType("nvarchar(256)");
            builder.Property(e => e.RpcConnection).HasColumnType("nvarchar(256)");
        }

        private void ConfigureChainInfo(EntityTypeBuilder<ChainInfo> builder)
        {
            builder.HasKey(x => x.ChainId);
            builder.Property(e => e.ChainId).ValueGeneratedNever();
            builder.HasIndex(x => x.ChainId)
                .IsUnique();
        }

        private void ConfigureDownloaderSettings(EntityTypeBuilder<DownloaderSettings> builder)
        {
            builder.HasKey(e => new { e.ChainId, e.ContractAddress, e.EventHash });

            builder.Property(e => e.ContractAddress).HasColumnType("nvarchar(42)");
            builder.Property(e => e.EventHash).HasColumnType("nvarchar(66)");
            builder.Property(e => e.Key).HasColumnType("nvarchar(256)");
            builder.Property(e => e.ResponseType).HasColumnType("nvarchar(256)").HasConversion(new EnumToStringConverter<ResponseType>());
        }

        private void ConfigureDownloaderMapping(EntityTypeBuilder<DownloaderMapping> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Path).HasColumnType("nvarchar(max)");
            builder.Property(e => e.Converter).HasColumnType("nvarchar(max)");
            builder.Property(e => e.Name).HasColumnType("nvarchar(256)");
        }
    }
}