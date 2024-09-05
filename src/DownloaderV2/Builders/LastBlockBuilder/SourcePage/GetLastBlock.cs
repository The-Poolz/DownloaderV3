using DownloaderV2.Helpers;
using DownloaderV2.HttpFlurlClient;
using DownloaderV2.Models.LastBlock;
using DownloaderV2.Utilities;
using EnvironmentManager.Extensions;
using Newtonsoft.Json.Linq;

namespace DownloaderV2.Builders.LastBlockBuilder.SourcePage;

public class GetLastBlock(string getUri) : GetSourcePage(getUri)
{
    public GetLastBlock() : this($"{Environments.LastBlockDownloaderUrl.Get<string>()}{Environments.LastBlockKey.Get<string>()}") { }

    public override async Task<JToken?> GetResponseAsync(string uri) => await Request.CovalentResponse(uri);

    public override LastBlockResponse DeserializeResponse(JToken jsonData)
    {
        var lastBlockData = jsonData.ToObject<LastBlockResponse>();

        return lastBlockData ?? ApplicationLogger.LogAndThrowDynamic(new InvalidOperationException(ExceptionMessages.FailedToRetrieveLastBlockData));
    }

    public override Dictionary<long, long> PopulateDataDictionary(LastBlockResponse downloadedLastBlockData)
    {
        return downloadedLastBlockData.Data.Items?
                   .ToDictionary(item => item.ChainId, item => item.BlockHeight)
               ?? new Dictionary<long, long>();
    }
}