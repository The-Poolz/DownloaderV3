namespace DownloaderV3.Source.CovalentLastBlock.SourcePage;

public abstract class GetSourcePage
{
    public virtual Dictionary<long, long> FetchData()
    {
        var jsonData = GetResponse();
        return ParseResponse(jsonData!);
    }

    public abstract string? GetResponse();

    public abstract Dictionary<long, long> ParseResponse(string jsonData);
}