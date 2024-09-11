using Newtonsoft.Json.Linq;
using DownloaderV3.Models.Covalent;
using DownloaderV3.Destination.Models;

namespace DownloaderV3.Decoders;

public class TopicDecoder
{
    public TopicDecoder(Data data, IEnumerable<DownloaderMapping> downloaderMappings)
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