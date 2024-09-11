using Nethereum.Util;
using System.Numerics;

namespace DownloaderV3.Decoders.DataDecoders;

public class HexToDecimalNegative : DataDecoder
{
    private const int Decimals = 18;
    public override void BuildFromData(string topicData)
    {
        var bigIntValue = BigInteger.Parse(topicData, System.Globalization.NumberStyles.HexNumber);
        var result = new BigDecimal(bigIntValue) / new BigDecimal(BigInteger.Pow(10, Decimals));

        DecodedData = decimal.Round((decimal)result, Decimals);
    }
}