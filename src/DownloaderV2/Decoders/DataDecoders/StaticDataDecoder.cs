using Newtonsoft.Json.Linq;

namespace DownloaderV2.Decoders.DataDecoders;

public class StaticDataDecoder : DataDecoder
{
    public override void Initialize(DecoderConfiguration conf, JToken source)
    {
        this.BuildFromData(GetTopicData(conf, source));
    }

    public override string GetTopicData(DecoderConfiguration conf, JToken source) => conf.Parameters.DecoderInDecoder!;
}