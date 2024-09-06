using Newtonsoft.Json.Linq;
using DownloaderV2.Helpers;
using DownloaderV2.Utilities;
using DownloaderV2.HttpFlurlClient;
using DownloaderV2.Models.LastBlock;
using EnvironmentManager.Extensions;

namespace DownloaderV2.Builders.LastBlockBuilder.SourcePage;

public class GetLastBlock(string getUri) : GetSourcePage
{
    public string GetUri { get; } = getUri;

    public GetLastBlock() : this($"{Environments.LastBlockDownloaderUrl.Get<string>()}{Environments.LastBlockKey.Get<string>()}") { }

    public override async Task<Dictionary<long, long>> FetchDataAsync()
    {
        var jsonData = await GetResponseAsync(GetUri);
        return ParseResponse(jsonData!);
    }

    public override async Task<JToken?> GetResponseAsync(string uri) => await Request.CovalentResponse(uri);

    public override Dictionary<long, long> ParseResponse(JToken jsonData)
    {
        var lastBlockData = jsonData.ToObject<LastBlockResponse>();

        if (lastBlockData == null)
        {
            ApplicationLogger.LogAndThrowDynamic(new InvalidOperationException(ExceptionMessages.FailedToRetrieveLastBlockData));
            return new Dictionary<long, long>();
        }

        return lastBlockData.Data.Items?
                   .ToDictionary(item => item.ChainId, item => item.BlockHeight)
               ?? new Dictionary<long, long>();
    }
}