using DownloaderV3.Utilities;
using EnvironmentManager.Extensions;
using Net.Cache.DynamoDb.ERC20;
using Net.Cache.DynamoDb.ERC20.Models;

namespace DownloaderV3.Helpers;

public class ERC20CacheManager(ERC20CacheProvider erc20CacheProvider)
{
    public ERC20CacheManager() : this(new ERC20CacheProvider()) { }

    public ERC20DynamoDbTable GetTokenInfo(long chainId, string tokenAddress)
    {
        var cacheRequest = new ApiRequestFactory().CreateCacheRequest(Environments.LastBlockKey.Get<string>(), chainId, tokenAddress, Environments.ApiUrl.Get<string>());

        return erc20CacheProvider.GetOrAdd(cacheRequest);
    }
}