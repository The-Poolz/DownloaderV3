using Newtonsoft.Json.Linq;
using DownloaderV2.Models.LastBlock;

namespace DownloaderV2.Builders.LastBlockBuilder.SourcePage;

public abstract class GetSourcePage(string getUri)
{
    public readonly string GetUri = getUri;

    public abstract Task<Dictionary<long, long>> FetchDataAsync();

    public abstract Task<JToken?> GetResponseAsync(string uri);

    public abstract LastBlockResponse DeserializeResponse(JToken jsonData);

    public abstract Dictionary<long, long> PopulateDataDictionary(LastBlockResponse downloadedData);
}