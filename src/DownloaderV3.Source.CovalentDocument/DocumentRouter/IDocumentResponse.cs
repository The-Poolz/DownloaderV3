using DownloaderV3.Destination;

namespace DownloaderV3.Source.CovalentDocument.DocumentRouter;

public interface IDocumentResponse
{
    public void Save(BaseDestination destination);
}