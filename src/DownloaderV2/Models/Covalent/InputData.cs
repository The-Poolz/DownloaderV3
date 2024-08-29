using Newtonsoft.Json;

namespace DownloaderV2.Models.Covalent;

public class InputData
{
    public Data Data { get; set; } = null!;

    public bool Error { get; set; }

    [JsonProperty("Error_message")]
    public string? ErrorMessage { get; set; }

    [JsonProperty("Error_code")]
    public int? ErrorCode { get; set; }
}