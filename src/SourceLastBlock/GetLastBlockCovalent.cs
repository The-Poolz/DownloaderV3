using Flurl.Http;
using Newtonsoft.Json;
using SourceLastBlock.Helpers;
using SourceLastBlock.Utilities;
using SourceLastBlock.SourcePage;
using EnvironmentManager.Extensions;
using SourceLastBlock.Models.LastBlock;

namespace SourceLastBlock;

public class GetLastBlockCovalent : GetSourcePage
{
    private string GetUri { get; } = $"{Environments.LastBlockDownloaderUrl.Get<string>()}{Environments.LastBlockKey.Get<string>()}";
    public override string? GetResponse() => GetUri.GetStringAsync().GetAwaiter().GetResult();

    public override Dictionary<long, long> ParseResponse(string jsonData)
    {
        var lastBlockData = JsonConvert.DeserializeObject<LastBlockResponse>(jsonData);

        if (lastBlockData?.Data == null)
            throw new InvalidOperationException(ExceptionMessages.FailedToRetrieveLastBlockData);

        return lastBlockData.Data.Items?
                   .ToDictionary(item => item.ChainId, item => item.BlockHeight)
               ?? new Dictionary<long, long>();
    }
}