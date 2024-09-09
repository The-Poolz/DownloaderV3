using Newtonsoft.Json.Linq;
using SourceLastBlock.Helpers;
using SourceLastBlock.Utilities;
using EnvironmentManager.Extensions;
using SourceLastBlock.HttpFlurlClient;
using SourceLastBlock.Models.LastBlock;
using SourceLastBlock.AbstractClass;

namespace SourceLastBlock
{
    public class GetLastBlock : GetSourcePage
    {
        private string GetUri { get; } = $"{Environments.LastBlockDownloaderUrl.Get<string>()}{Environments.LastBlockKey.Get<string>()}";
        public override async Task<JToken?> GetResponseAsync() => await Request.CovalentResponse(GetUri);

        public override Dictionary<long, long> ParseResponse(JToken jsonData)
        {
            var lastBlockData = jsonData.ToObject<LastBlockResponse>();

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