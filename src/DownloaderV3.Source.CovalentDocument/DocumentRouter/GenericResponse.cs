using DownloaderV3.Destination;
using DownloaderV3.Source.CovalentDocument.Helpers;
using DownloaderV3.Source.CovalentDocument.Decoders;

namespace DownloaderV3.Source.CovalentDocument.DocumentRouter;

public class GenericResponse<T> : IDocumentResponse where T : class, new()
{
    public T Instance { get; } = new();
    private IBeforeSave? BeforeSave { get; }

    public GenericResponse(IEnumerable<IReadOnlyDictionary<string, DataDecoder>> listOfData, IBeforeSave? beforeSave = null)
    {
        BeforeSave = beforeSave;
        listOfData.ToList().ForEach(item => ObjectMaker.MakeObject(Instance, item));
    }

    public void Save(BaseDestination destination)
    {
        BeforeSave?.Run(Instance);
        destination.Set<T>().Add(Instance);
    }
}