using Newtonsoft.Json;

namespace DownloaderV3.Source.CovalentDocument.Models.Covalent;

public class Transaction
{
    [JsonProperty("Block_signed_at")]
    public DateTime BlockSignedAt { get; set; }

    [JsonProperty("Block_height")]
    public int BlockHeight { get; set; }

    [JsonProperty("Tx_offset")]
    public int TxOffset { get; set; }

    [JsonProperty("Log_offset")]
    public int LogOffset { get; set; }

    [JsonProperty("Tx_hash")]
    public string TxHash { get; set; } = null!;

    [JsonProperty("Raw_log_topics")]
    public string[] RawLogTopics { get; set; } = null!;

    [JsonProperty("Sender_contract_decimals")]
    public int? SenderContractDecimals { get; set; }

    [JsonProperty("Sender_name")]
    public string? SenderName { get; set; }

    [JsonProperty("Sender_contract_ticker_symbol")]
    public string? SenderContractTickerSymbol { get; set; }

    [JsonProperty("Sender_address")]
    public string SenderAddress { get; set; } = null!;

    [JsonProperty("Sender_address_label")]
    public string? SenderAddressLabel { get; set; }

    [JsonProperty("Sender_logo_url")]
    public string SenderLogoUrl { get; set; } = null!;

    [JsonProperty("Raw_log_data")]
    public string RawLogData { get; set; } = null!;

    public Decoded? Decoded { get; set; }
}