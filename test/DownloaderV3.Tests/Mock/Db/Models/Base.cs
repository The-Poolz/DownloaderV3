using System.ComponentModel.DataAnnotations.Schema;

namespace DownloaderV3.Tests.Mock.Db.Models;

public class Base
{
    public long ChainId { get; set; }

    public long BlockHeight { get; set; }

    [Column(TypeName = "nvarchar(66)")]
    public string TxHash { get; set; } = null!;

    [Column(TypeName = "datetime2(0)")]
    public DateTime BlockSignedAt { get; set; }

    [Column(TypeName = "nvarchar(42)")]
    public string SenderAddress { get; set; } = null!;

    public long TxOffset { get; set; }

    public long LogOffset { get; set; }
}
