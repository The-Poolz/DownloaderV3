using Newtonsoft.Json;

namespace DownloaderV3.Source.CovalentLastBlock.Models.LastBlock;

public class Item
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("chain_id")]
    public Int64 ChainId { get; set; }

    [JsonProperty("is_testnet")]
    public bool IsTestnet { get; set; }

    [JsonProperty("logo_url")]
    public string? LogoUrl { get; set; }

    [JsonProperty("synced_block_height")]
    public Int64 BlockHeight { get; set; }

    [JsonProperty("synced_blocked_signed_at")]
    public DateTime SignedAt { get; set; }

    [JsonProperty("has_data")]
    public bool HasData { get; set; }
}