using DownloaderV3.Destination.Models;

namespace DownloaderV3.Tests.Results.DbResults;

public static class DownloaderSettingsResult
{
    private const string Url = "https://api/test";

    public static readonly DownloaderSettings SomeEvent = new()
    {
        ChainId = 97
    };

    public static readonly DownloaderSettings SomeAnotherEvent = new()
    {
        ChainId = 97
    };
}