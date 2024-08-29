using DownloaderContext.Models;

namespace DownloaderV2.Result;

public class ResultObject
{
    public long ChainId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public int Count { get; set; }

    public ResultObject SetSuccess(DownloaderSettings settings, int count)
    {
        ChainId = settings.ChainId;
        EventName = settings.ResponseType.ToString();
        Count = count;
        return this;
    }

    public override string ToString()
    {
        return $"| {Count} saved | ChainID {ChainId,6} | {EventName}\n";
    }
}