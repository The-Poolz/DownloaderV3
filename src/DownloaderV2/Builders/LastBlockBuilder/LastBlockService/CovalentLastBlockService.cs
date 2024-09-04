using DownloaderV2.Helpers;
using DownloaderV2.Utilities;
using DownloaderV2.HttpFlurlClient;
using DownloaderV2.Models.LastBlock;
using EnvironmentManager.Extensions;

namespace DownloaderV2.Builders.LastBlockBuilder.LastBlockService;

public class CovalentLastBlockService : ILastBlockService
{
    public readonly string GetUri = $"{Environments.LastBlockDownloaderUrl.Get<string>()}{Environments.LastBlockKey.Get<string>()}";
    private Dictionary<long, long>? _lastBlockDictionary = new();

    public async Task<Dictionary<long, long>> FetchLastBlockDataAsync()
    {
        if (_lastBlockDictionary is { Count: > 0 })
            return _lastBlockDictionary;

        var jsonData = await Request.CovalentResponse(GetUri);
        var downloadedLastBlockData = jsonData?.ToObject<LastBlockResponse>() ??
                                      ApplicationLogger.LogAndThrowDynamic(new InvalidOperationException(ExceptionMessages.FailedToRetrieveLastBlockData));

        _lastBlockDictionary = PopulateLastBlockDictionary(downloadedLastBlockData);
        return _lastBlockDictionary;
    }

    private Dictionary<long, long> PopulateLastBlockDictionary(LastBlockResponse downloadedLastBlockData)
    {
        var dictionary = new Dictionary<long, long>();
        foreach (var item in downloadedLastBlockData.Data.Items ?? Array.Empty<Item>()) dictionary[item.ChainId] = item.BlockHeight;

        return dictionary;
    }
}