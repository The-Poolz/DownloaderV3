namespace DownloaderV2.Models.ApiCovalent;

public interface IPagination
{
    bool HasMore { get; set; }
    int PageNumber { get; set; }
    int PageSize { get; set; }
    int? TotalCount { get; set; }
}