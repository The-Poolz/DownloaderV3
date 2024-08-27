namespace DownloaderV2.Models.ApiCovalent;

public interface IData
{
    long ChainId { get; set; }
    DateTime UpdatedAt { get; set; }
    ITransaction[] Items { get; set; }
    IPagination Pagination { get; set; }
    IData Clone();
}