using DownloaderV3.Helpers;
using DownloaderV3.Decoders;
using DownloaderV3.Destination;

namespace DownloaderV3.LogRouter;

public class GenericResponse<T> : ILogResponse where T : class, new()
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