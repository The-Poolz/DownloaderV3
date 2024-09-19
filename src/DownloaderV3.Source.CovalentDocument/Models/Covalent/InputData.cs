using Newtonsoft.Json;

namespace DownloaderV3.Source.CovalentDocument.Models.Covalent;

public class InputData : IHasPagination
{
    public Data Data { get; set; } = null!;

    public bool Error { get; set; }

    [JsonProperty("Error_message")]
    public string? ErrorMessage { get; set; }

    [JsonProperty("Error_code")]
    public int? ErrorCode { get; set; }

    public Pagination Pagination => Data.Pagination;
}