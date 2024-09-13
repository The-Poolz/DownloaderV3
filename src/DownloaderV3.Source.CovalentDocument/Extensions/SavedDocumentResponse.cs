using DownloaderV3.Destination;
using DownloaderV3.Source.CovalentDocument.DocumentRouter;

namespace DownloaderV3.Source.CovalentDocument.Extensions;

public class SavedDocumentResponse : List<IDocumentResponse>
{
    public void LockedSaveAll(BaseDestination destination)
    {
        ForEach(response =>
        {
            lock (destination)
            {
                response.Save(destination);
            }
        });
    }
}