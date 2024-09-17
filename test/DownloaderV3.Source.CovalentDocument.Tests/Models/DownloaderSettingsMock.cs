using DownloaderV3.Destination.Models;

namespace DownloaderV3.Source.CovalentDocument.Tests.Models;

public static class DownloaderSettingsMock
{
    public static DownloaderSettings DownloaderSettings = new DownloaderSettings
    {
        ChainId = 56,
        StartingBlock = 1,
        EndingBlock = 200,
        MaxBatchSize = 50000,
        Key = "myKey",
        ResponseType = "ResponseType",
        ContractAddress = "0x1Fa306AAfcbf6B5e13a19c02E044ee28588B0000",
        EventHash = "0xc42079f94a6350d7e6235f29174924f928cc2ac818eb64fed8004e115fbc00007",
        Active = true,
        MaxPageNumber = 1000,
        UrlSet = "https://api/{{ChainId}}/events/address/{{ContractAddress}}/?starting-block={{StartingBlock}}&ending-block={{EndingBlock}}&page-number={{PageNumber}}&page-size={{MaxPageNumber}}&key={{Key}}"
    };

    public static DownloaderMapping[] DownloaderMappings = new DownloaderMapping[]
    {
        new() { Name = "ChainId", Converter = "StringToInt32", Path = "$.chain_id", DownloaderSettings = DownloaderSettings },
        new() { Name = "BlockHeight", Converter = "StringToInt32", Path = "$.Items[0].Block_height", DownloaderSettings = DownloaderSettings },
    };
}