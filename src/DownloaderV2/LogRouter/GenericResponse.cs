using DownloaderV2.Helpers;
using DownloaderV2.Decoders;
using Microsoft.EntityFrameworkCore;

namespace DownloaderV2.LogRouter;

public class GenericResponse<T> : ILogResponse where T : class, new()
{
    public T Instance { get; } = new();
    private IBeforeSave? BeforeSave { get; }

    public GenericResponse(IEnumerable<IReadOnlyDictionary<string, DataDecoder>> listOfData, IBeforeSave? beforeSave = null)
    {
        BeforeSave = beforeSave;
        listOfData.ToList().ForEach(item => ObjectMaker.MakeObject(Instance, item));
    }

    public void Save(DbContext context)
    {
        BeforeSave?.Run(Instance);
        context.Set<T>().Add(Instance);
    }
}