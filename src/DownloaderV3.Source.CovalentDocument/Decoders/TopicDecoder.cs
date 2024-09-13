using Newtonsoft.Json.Linq;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument.Decoders;

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