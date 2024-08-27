using Newtonsoft.Json.Linq;
using DownloaderContext.Models;
using DownloaderV2.Models.ApiCovalent;

namespace DownloaderV2.Decoders;

public class TopicDecoder
{
    public TopicDecoder(IData data, IEnumerable<DownloaderMapping> downloaderMappings)
    {
        var source = JObject.FromObject(data);

        foreach (var map in downloaderMappings)
        {
            var decoder = new DataDecoderFactory(map, source).Decoder;
            LogData.Add(map.Name, decoder);
        }
    }

    public Dictionary<string, DataDecoder> LogData { get; } = new();
}