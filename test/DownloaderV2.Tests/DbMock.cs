using DownloaderContext.Models;
using DownloaderV2.Tests.Results.DbResults.CustomDownloaderContext;
using Microsoft.EntityFrameworkCore;

namespace DownloaderV2.Tests;

internal static class DbMock
{
    public static async Task<CustomDownloaderContext> CreateMockContextAsync()
    {
        var options = new DbContextOptionsBuilder<CustomDownloaderContext>()
            .UseInMemoryDatabase(databaseName: "TestDb" + Guid.NewGuid().ToString())
            .Options;

        var context = new CustomDownloaderContext(options);

        var swapBNBParty = new DownloaderSettings
        {
            ChainId = 97,
            StartingBlock = 1,
            EndingBlock = 100,
            MaxBatchSize = 50000,
            Key = "myKey",
            ResponseType = "SwapBNBParty",
            ContractAddress = "0x1Fa306AAfcbf6B5e13a19c02E044ee28588B48ae",
            EventHash = "0xc42079f94a6350d7e6235f29174924f928cc2ac818eb64fed8004e115fbcca67",
            Active = true,
            MaxPageNumber = 1000,
            UrlSet = "https://api.covalenthq.com/v1/[ChainId]/events/address/[ContractAddress]/?starting-block=[StartingBlock]&ending-block=[EndingBlock]&page-number=[PageNumber]&page-size=[MaxPageNumber]&key=[Key]"
        };
        var downloaderMappings = new DownloaderMapping[]
        {
            new() { Name = "ChainId", Converter = "StringToInt32", Path = "$.chain_id", DownloaderSettings = swapBNBParty },
            new() { Name = "BlockHeight", Converter = "StringToInt32", Path = "$.Items[0].Block_height", DownloaderSettings = swapBNBParty },
            new() { Name = "TxHash", Converter = "HexToString", Path = "$.Items[0].Tx_hash", DownloaderSettings = swapBNBParty },
            new() { Name = "SenderAddress", Converter = "HexToString", Path = "$.Items[0].Sender_address", DownloaderSettings = swapBNBParty },
            new() { Name = "BlockSignedAt", Converter = "StringToDateTime", Path = "$.Items[0].Block_signed_at", DownloaderSettings = swapBNBParty },
            new() { Name = "TxOffset", Converter = "StringToInt32", Path = "$.Items[0].Tx_offset", DownloaderSettings = swapBNBParty },
            new() { Name = "LogOffset", Converter = "StringToInt32", Path = "$.Items[0].Log_offset", DownloaderSettings = swapBNBParty },
            new() { Name = "Sender", Converter = "HexToAddress", Path = "$.Items[0].Raw_log_topics[1]", DownloaderSettings = swapBNBParty },
            new() { Name = "Recipient", Converter = "HexToAddress", Path = "$.Items[0].Raw_log_topics[2]", DownloaderSettings = swapBNBParty },
            new() { Name = "Amount0", Converter = "RawDataDecoder(HexToDecimalNegative)", Path = "$.Items[0].Raw_log_data#1", DownloaderSettings = swapBNBParty },
            new() { Name = "Amount1", Converter = "RawDataDecoder(HexToDecimalNegative)", Path = "$.Items[0].Raw_log_data#2", DownloaderSettings = swapBNBParty },
            new() { Name = "Liquidity", Converter = "RawDataDecoder(HexToDecimal)", Path = "$.Items[0].Raw_log_data#4", DownloaderSettings = swapBNBParty },
            new() { Name = "Tick", Converter = "RawDataDecoder(HexToNumberNegative)", Path = "$.Items[0].Raw_log_data#5", DownloaderSettings = swapBNBParty }
        };

        var chains = new ChainInfo
        {
            ChainId = 97,
            BlockPerSecond = 0.333f,
            SecondsToWarning = 1000,
            SecondsToError = 1000,
            DownloadTimeDelay = 60
        };

        context.DownloaderSettings.Add(swapBNBParty);
        context.DownloaderMapping.AddRange(downloaderMappings);
        context.ChainsInfo.Add(chains);
        await context.SaveChangesAsync();

        return context;
    }
}