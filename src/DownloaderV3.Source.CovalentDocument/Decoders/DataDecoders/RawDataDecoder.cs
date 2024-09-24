using Newtonsoft.Json.Linq;

namespace DownloaderV3.Source.CovalentDocument.Decoders.DataDecoders;

public class RawDataDecoder : DataDecoder
{
    public override void Initialize(DecoderConfiguration conf, JToken source)
    {
        var converter = DataDecoderFactory.GetConverter(conf.Parameters.DecoderInDecoder!);
        converter.BuildFromData(GetTopicData(conf, source));
        DecodedData = converter.DecodedData;
    }

    public override string GetTopicData(DecoderConfiguration conf, JToken source) => new ChunkExtractor(conf.Mapping.Path, source).TopicData;
}