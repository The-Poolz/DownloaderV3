using DownloaderV2.Helpers;
using Newtonsoft.Json.Linq;
using Nethereum.Hex.HexTypes;

namespace DownloaderV2.Decoders.DataDecoders;

public class HexToDecimalWithTokenPath : HexToDecimal
{
    public int Decimals { get; set; }

    public override void Initialize(DecoderConfiguration conf, JToken source)
    {
        var mapClone = (DecoderConfiguration)conf.Clone();
        mapClone.Mapping.Path = conf.Parameters.DecoderInDecoder!;
        mapClone.Mapping.Converter = "HexToAddress";

        var addressConverter = DataDecoderFactory.GetConverter(nameof(HexToAddress));
        addressConverter.Initialize(mapClone, source);
        DecodedData = addressConverter.DecodedData;

        Decimals = new ERC20CacheManager().GetTokenInfo(conf.Mapping.DownloaderSettings.ChainId, DecodedData).Decimals;

        BuildFromData(GetTopicData(conf, source));
    }

    public override void BuildFromData(string topicData) => DecodedData = Nethereum.Util.UnitConversion.Convert.FromWei(new HexBigInteger(topicData).Value, Decimals);
}