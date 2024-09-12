using Newtonsoft.Json;

namespace DownloaderV3.Source.CovalentLastBlock.Models.LastBlock;

public class Data
{
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonProperty("items")]
    public Item[]? Items { get; set; }

    [JsonProperty("pagination")]
    public string Pagination { get; set; } = null!;
}