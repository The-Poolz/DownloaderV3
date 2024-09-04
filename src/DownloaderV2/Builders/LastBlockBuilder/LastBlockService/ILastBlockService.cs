namespace DownloaderV2.Builders.LastBlockBuilder.LastBlockService;

public interface ILastBlockService
{
    Task<Dictionary<long, long>> FetchLastBlockDataAsync();
}