using Newtonsoft.Json;
using SourceLastBlock.Helpers;
using SourceLastBlock.Utilities;
using SourceLastBlock.SourcePage;
using EnvironmentManager.Extensions;
using SourceLastBlock.HttpFlurlClient;
using SourceLastBlock.Models.LastBlock;

namespace SourceLastBlock
{
    public class GetLastBlock : GetSourcePage
    {
        private string GetUri { get; } = $"{Environments.LastBlockDownloaderUrl.Get<string>()}{Environments.LastBlockKey.Get<string>()}";
        public override string? GetResponse() => Request.CovalentResponse(GetUri).GetAwaiter().GetResult();

        public override Dictionary<long, long> ParseResponse(string jsonData)
        {
            var lastBlockData = JsonConvert.DeserializeObject<LastBlockResponse>(jsonData);

            if (lastBlockData?.Data == null)
            {
                ApplicationLogger.LogAndThrowDynamic(new InvalidOperationException(ExceptionMessages.FailedToRetrieveLastBlockData));
                return new Dictionary<long, long>();
            }

            return lastBlockData.Data.Items?
                       .ToDictionary(item => item.ChainId, item => item.BlockHeight)
                   ?? new Dictionary<long, long>();
        }
    }
}