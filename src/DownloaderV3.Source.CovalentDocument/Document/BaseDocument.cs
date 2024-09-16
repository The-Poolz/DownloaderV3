namespace DownloaderV3.Source.CovalentDocument.Document;

public abstract class BaseDocument<TData>
{
    public TData? DownloadedContractData { get; protected set; }
    public long SavedLastBlock { get; protected set; }
    public long SourceLastBlock { get; protected set; }

    public virtual void FetchData()
    {
        var sourceData = GetResponse();
        DownloadedContractData = DeserializeResponse(sourceData) ?? throw new InvalidOperationException("Failed to deserialize response.");
    }

    public abstract string GetResponse();

    public abstract TData? DeserializeResponse(string sourceData);
}