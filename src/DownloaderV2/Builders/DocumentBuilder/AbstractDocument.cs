using Newtonsoft.Json.Linq;
using DownloaderV2.Models.Covalent;

namespace DownloaderV2.Builders.DocumentBuilder;

public abstract class AbstractDocument(long savedLastBlock, long sourceLastBlock)
{
    public abstract string UrlSet { get; }
    public abstract string GetUrl { get; }
    public long SavedLastBlock { get; set; } = savedLastBlock;
    public long SourceLastBlock { get; set; } = sourceLastBlock;

    public abstract Task<InputData> FetchDataAsync();

    protected abstract JToken? SendRequest(string url);

    protected abstract InputData DeserializeResponse(JToken responseData);
}