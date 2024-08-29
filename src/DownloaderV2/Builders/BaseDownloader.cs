using Newtonsoft.Json.Linq;
using DownloaderV2.Utilities;
using DownloaderV2.HttpFlurlClient;
using EnvironmentManager.Extensions;

namespace DownloaderV2.Builders;

public class BaseDownloader
{
    protected BaseDownloader() => GetUri = $"{Environments.LastBlockDownloaderUrl.Get<string>()}{Environments.LastBlockKey.Get<string>()}";

    private string GetUri{ get; }
    protected JToken? DownloadData => Request.CovalentResponse(GetUri).GetAwaiter().GetResult();
}