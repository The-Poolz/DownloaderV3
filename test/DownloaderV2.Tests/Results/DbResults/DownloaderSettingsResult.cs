using DownloaderContext.Models;

namespace DownloaderV2.Tests.Results.DbResults;

public static class DownloaderSettingsResult
{
    private const string Url = "https://api.covalenthq.com/v1/[ChainId]/events/address/test";

    public static readonly DownloaderSettings SwapBNBPartyV1 = new()
    {
        ChainId = 97,
        StartingBlock = 1,
        EndingBlock = 2,
        MaxBatchSize = 50000,
        Key = "myKey",
        ResponseType = "SwapBNBParty",
        ContractAddress = "0x1Fa306AAfcbf6B5e13a19c02E044ee28588B48ae",
        EventHash = "0xc42079f94a6350d7e6235f29174924f928cc2ac818eb64fed8004e115fbcca67",
        Active = true,
        MaxPageNumber = 1000,
        UrlSet = Url
    };

    public static readonly DownloaderSettings SwapBNBPartyV2 = new()
    {
        ChainId = 97,
        StartingBlock = 1,
        EndingBlock = 2,
        MaxBatchSize = 50000,
        Key = "myKey",
        ResponseType = "SwapBNBParty",
        ContractAddress = "0x1Fa306AAfcbf6B5e13a19c02E044ee28588B333",
        EventHash = "0xc42079f94a6350d7e6235f29174924f928cc2ac818eb64fed8004e115fbcc333",
        Active = true,
        MaxPageNumber = 1000,
        UrlSet = Url
    };
}
