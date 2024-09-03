using System.ComponentModel.DataAnnotations.Schema;
using DownloaderContext.Attributes;

namespace DownloaderV2.Tests.Results.DbResults.CustomDownloaderContext.Models.ResponseModels;

[ResponseModel]
public class SwapBNBParty : ThePoolzBase
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