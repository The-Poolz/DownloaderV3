using Newtonsoft.Json;

namespace DownloaderV3.Models.Covalent;

public class Pagination
{
    [JsonProperty("Has_more")]
    public bool HasMore { get; set; }

    [JsonProperty("Page_number")]
    public int PageNumber { get; set; }

    [JsonProperty("Page_size")]
    public int PageSize { get; set; }

    [JsonProperty("Total_count")]
    public int? TotalCount { get; set; }
}