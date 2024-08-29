namespace DownloaderV2.Models.Api;

public interface IInputData
{
    IData Data { get; set; }
    bool Error { get; set; }
    string? ErrorMessage { get; set; }
    int? ErrorCode { get; set; }
}