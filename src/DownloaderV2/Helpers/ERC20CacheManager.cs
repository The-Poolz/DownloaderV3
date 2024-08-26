using Amazon;
using Amazon.DynamoDBv2;
using DownloaderV2.Decoders;
using DownloaderV2.Utilities;
using Net.Cache.DynamoDb.ERC20;
using Amazon.DynamoDBv2.DataModel;
using Net.Cache.DynamoDb.ERC20.Models;

namespace DownloaderV2.Helpers;

public class ERC20CacheManager : DataDecoder
{
    private readonly ERC20CacheProvider _erc20CacheProvider;

    public ERC20CacheManager(ERC20CacheProvider erc20CacheProvider)
    {
        _erc20CacheProvider = erc20CacheProvider;
    }

    public ERC20CacheManager()
    {
        var region = RegionEndpoint.GetBySystemName(Environments.AwsRegion.Get());
        var client = new AmazonDynamoDBClient(region);
        var context = new DynamoDBContext(client);
        var erc20StorageProvider = new ERC20StorageProvider(context);
        _erc20CacheProvider = new ERC20CacheProvider(erc20StorageProvider);
    }

    public ERC20DynamoDbTable GetTokenInfo(long chainId, string tokenAddress)
    {
        var cacheRequest = new ApiRequestFactory().CreateCacheRequest(Environments.LastBlockKey.Get(), chainId, tokenAddress, Environments.ApiUrl.Get());

        return _erc20CacheProvider.GetOrAdd(cacheRequest);
    }
}