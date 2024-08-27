namespace DownloaderV2.Models.ApiCovalent;

public interface ITransaction
{
    DateTime BlockSignedAt { get; set; }
    int BlockHeight { get; set; }
    int TxOffset { get; set; }
    int LogOffset { get; set; }
    string TxHash { get; set; }
    string[] RawLogTopics { get; set; }
    int? SenderContractDecimals { get; set; }
    string? SenderName { get; set; }
    string? SenderContractTickerSymbol { get; set; }
    string SenderAddress { get; set; }
    string? SenderAddressLabel { get; set; }
    string SenderLogoUrl { get; set; }
    string RawLogData { get; set; }
    IDecoded? Decoded { get; set; }
}