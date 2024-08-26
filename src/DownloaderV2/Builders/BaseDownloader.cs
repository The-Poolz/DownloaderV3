using Newtonsoft.Json.Linq;
using DownloaderV2.Utilities;
using DownloaderV2.HttpFlurlClient;

namespace DownloaderV2.Builders;

public class BaseDownloader
{
    protected BaseDownloader()
    {
        GetUri = $"{Environments.LastBlockDownloaderUrl.Get()}{Environments.LastBlockKey.Get()}";
    }

    private string GetUri{ get; }
    protected JToken? DownloadData => Request.CovalentResponse(GetUri).GetAwaiter().GetResult();
}