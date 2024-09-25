using DownloaderV3.Destination.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace DownloaderV3.Tests.Mock.Db.Models;

[ResponseModel]
public class SwapParty : Base
{
    [Column(TypeName = "nvarchar(42)")]
    public string Sender { get; set; } = null!;

    [Column(TypeName = "nvarchar(42)")]
    public string Recipient { get; set; } = null!;

    [Column(TypeName = "decimal(36,18)")]
    public decimal Amount0 { get; set; }

    [Column(TypeName = "decimal(36,18)")]
    public decimal Amount1 { get; set; }

    [Column(TypeName = "decimal(36,18)")]
    public decimal Liquidity { get; set; }

    [Column(TypeName = "bigint")]
    public long Tick { get; set; }
}