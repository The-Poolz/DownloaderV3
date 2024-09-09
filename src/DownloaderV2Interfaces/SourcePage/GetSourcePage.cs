using Newtonsoft.Json.Linq;

namespace DownloaderV2Interfaces.SourcePage;

public abstract class GetSourcePage
{
    public async Task<Dictionary<long, long>> FetchDataAsync()
    {
        var jsonData = await GetResponseAsync();
        return ParseResponse(jsonData!);
    }

    public abstract Task<JToken?> GetResponseAsync();

    public abstract Dictionary<long, long> ParseResponse(JToken jsonData);
}