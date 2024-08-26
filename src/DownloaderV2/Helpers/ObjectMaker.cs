using System.Reflection;
using DownloaderV2.Decoders;

namespace DownloaderV2.Helpers;

public static class ObjectMaker
{
    public static void MakeObject<T>(T obj, IReadOnlyDictionary<string, DataDecoder> dataList)
    {
        typeof(T)
          .GetProperties(BindingFlags.Public | BindingFlags.Instance)
          .Where(p => dataList.Any(d => d.Key == p.Name))
          .ToList()
          .ForEach(p => p.SetValue(obj, dataList.First(d => d.Key == p.Name).Value.DecodedData));
    }
}