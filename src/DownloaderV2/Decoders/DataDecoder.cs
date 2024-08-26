using Newtonsoft.Json.Linq;

namespace DownloaderV2.Decoders;

public abstract class DataDecoder
{
    public dynamic DecodedData { get; internal set; } = null!;

    protected DataDecoder() { }

    public virtual void Initialize(DecoderConfiguration conf, JToken source) => BuildFromData(GetTopicData(conf, source));

    public virtual string GetTopicData(DecoderConfiguration conf, JToken source)
    {
        var token = source.SelectToken(conf.Mapping.Path);
        return token == null
            ? throw new InvalidOperationException($"Token not found at path: {conf.Mapping.Path}")
            : token.ToString();
    }

    public virtual void BuildFromData(string topicData) => DecodedData = topicData ;
}