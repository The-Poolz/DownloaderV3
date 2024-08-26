using Nethereum.Hex.HexTypes;

namespace DownloaderV2.Decoders.DataDecoders;

public class HexToDecimal : DataDecoder
{
    private const int Decimals = 18;

    public override void BuildFromData(string topicData) => DecodedData = Nethereum.Util.UnitConversion.Convert.FromWei(new HexBigInteger(topicData).Value, Decimals);
}