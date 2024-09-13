using System.Numerics;
using Nethereum.Hex.HexTypes;

namespace DownloaderV3.Source.CovalentDocument.Decoders.DataDecoders;

public class HexToBool : DataDecoder
{
    public override void BuildFromData(string rawData)
    {
        var value = BigInteger.Parse(new HexUTF8String(rawData).Value, System.Globalization.NumberStyles.HexNumber);
        DecodedData = value != BigInteger.Zero;
    }
}