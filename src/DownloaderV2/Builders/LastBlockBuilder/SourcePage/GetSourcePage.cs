using Newtonsoft.Json.Linq;

namespace DownloaderV2.Builders.LastBlockBuilder.SourcePage;

public abstract class GetSourcePage
{
    public abstract Task<Dictionary<long, long>> FetchDataAsync();

    public abstract Task<JToken?> GetResponseAsync(string uri);

    public abstract Dictionary<long, long> ParseResponse(JToken jsonData);
}