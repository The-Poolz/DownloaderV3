using Newtonsoft.Json.Linq;
using DownloaderV2.Utilities;
using DownloaderV2.HttpFlurlClient;
using EnvironmentManager.Extensions;

namespace DownloaderV2.Builders;

public class BaseDownloader
{
    public string GetUri{ get; } = $"{Environments.LastBlockDownloaderUrl.Get<string>()}{Environments.LastBlockKey.Get<string>()}";

    protected JToken? DownloadData => Request.CovalentResponse(GetUri).GetAwaiter().GetResult();
}