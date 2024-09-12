using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument.Document;

public abstract class BaseDocument
{
    public InputData DownloadedContractData { get; protected set; } = null!;

    public virtual void FetchData()
    {
        var sourceData = GetResponse();
        DownloadedContractData = DeserializeResponse(sourceData!) ?? throw new InvalidOperationException("Failed to deserialize response.");
    }

    public abstract string? GetResponse();

    public abstract InputData? DeserializeResponse(string sourceData);
}