using DownloaderV3.Destination.Models;
using Chain = DownloaderV3.Destination.Models.Chain;

namespace DownloaderV3.Tests.Mock
{
    public static class MockSettings
    {
        public static DownloaderSettings DownloaderSettings = new DownloaderSettings
        {
            ChainId = 97,
            StartingBlock = 1,
            EndingBlock = 200,
            MaxBatchSize = 50000,
            Key = "myKey",
            ResponseType = "SwapParty",
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
            new() { Name = "TxHash", Converter = "HexToString", Path = "$.Items[0].Tx_hash", DownloaderSettings = DownloaderSettings },
            new() { Name = "SenderAddress", Converter = "HexToString", Path = "$.Items[0].Sender_address", DownloaderSettings = DownloaderSettings },
            new() { Name = "BlockSignedAt", Converter = "StringToDateTime", Path = "$.Items[0].Block_signed_at", DownloaderSettings = DownloaderSettings },
            new() { Name = "TxOffset", Converter = "StringToInt32", Path = "$.Items[0].Tx_offset", DownloaderSettings = DownloaderSettings },
            new() { Name = "LogOffset", Converter = "StringToInt32", Path = "$.Items[0].Log_offset", DownloaderSettings = DownloaderSettings },
            new() { Name = "Sender", Converter = "HexToAddress", Path = "$.Items[0].Raw_log_topics[1]", DownloaderSettings = DownloaderSettings },
            new() { Name = "Recipient", Converter = "HexToAddress", Path = "$.Items[0].Raw_log_topics[2]", DownloaderSettings = DownloaderSettings },
        };

        public static DownloaderMapping[] DownloaderMappingsAdvanced = new DownloaderMapping[]
        {
            new() { Name = "TotalAmount", Converter = "HexToDecimalWithTokenPath($.Items[0].Raw_log_topics[1])", Path = "$.Items[0].Raw_log_topics[3]", DownloaderSettings = DownloaderSettings }
        };

        public static Chain Chain = new Chain
        {
            ChainId = 97,
            Name = "Test",
            RpcConnection = "https://rpc.test"
        };

        public static ChainInfo ChainInfo = new ChainInfo
        {
            ChainId = 97,
            BlockPerSecond = 10,
            SecondsToWarning = 12,
            SecondsToError = 15,
            DownloadTimeDelay = 100,
        };
    }
}