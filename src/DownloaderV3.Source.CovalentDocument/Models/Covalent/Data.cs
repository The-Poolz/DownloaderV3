using Newtonsoft.Json;

namespace DownloaderV3.Source.CovalentDocument.Models.Covalent;

public class Data
{
    [JsonProperty("chain_id")]
    public long ChainId { get; set; }
    [JsonProperty("Updated_at")]
    public DateTime UpdatedAt { get; set; }
    public Transaction[] Items { get; set; } = null!;
    public Pagination Pagination { get; set; } = null!;

    public Data Clone() => (Data)MemberwiseClone();
}