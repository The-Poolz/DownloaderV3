using DownloaderContext.Models;

namespace DownloaderV2.Tests.Results.DbResults;

public static class DownloaderSettingsResult
{
    private const string Url = "https://api/test";

    public static readonly DownloaderSettings SomeEvent = new()
    {
        ChainId = 97,
        StartingBlock = 1,
        EndingBlock = 2,
        MaxBatchSize = 50000,
        Key = "myKey",
        ResponseType = "SomeEvent",
        ContractAddress = "SomeContractAddress",
        EventHash = "SomeEventHash",
        Active = true,
        MaxPageNumber = 1000,
        UrlSet = Url
    };

    public static readonly DownloaderSettings SomeAnotherEvent = new()
    {
        ChainId = 97,
        StartingBlock = 1,
        EndingBlock = 2,
        MaxBatchSize = 50000,
        Key = "myKey",
        ResponseType = "SomeAnotherEvent",
        ContractAddress = "SomeAnotherContractAddress",
        EventHash = "SomeEventHash",
        Active = true,
        MaxPageNumber = 1000,
        UrlSet = Url
    };
}
