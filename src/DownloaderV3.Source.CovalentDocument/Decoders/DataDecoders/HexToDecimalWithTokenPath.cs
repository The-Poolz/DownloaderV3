using Newtonsoft.Json.Linq;
using Nethereum.Hex.HexTypes;
using DownloaderV3.Source.CovalentDocument.Helpers;

namespace DownloaderV3.Source.CovalentDocument.Decoders.DataDecoders;

public class HexToDecimalWithTokenPath(ERC20CacheManager erc20CacheManager) : HexToDecimal
{
    private readonly ERC20CacheManager _erc20CacheManager = erc20CacheManager;

    public HexToDecimalWithTokenPath() : this(new ERC20CacheManager()) { } 

    public int Decimals { get; set; }

    public override void Initialize(DecoderConfiguration conf, JToken source)
    {
        var mapClone = (DecoderConfiguration)conf.Clone();
        mapClone.Mapping.Path = conf.Parameters.DecoderInDecoder!;
        mapClone.Mapping.Converter = "HexToAddress";

        var addressConverter = DataDecoderFactory.GetConverter(nameof(HexToAddress));
        addressConverter.Initialize(mapClone, source);
        DecodedData = addressConverter.DecodedData;

        Decimals = _erc20CacheManager.GetTokenInfo(conf.Mapping.DownloaderSettings.ChainId, DecodedData).Decimals;
        BuildFromData(GetTopicData(conf, source));
    }

    public override void BuildFromData(string topicData) => DecodedData = Nethereum.Util.UnitConversion.Convert.FromWei(new HexBigInteger(topicData).Value, Decimals);
}