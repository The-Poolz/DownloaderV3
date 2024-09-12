using System.Numerics;
using System.Globalization;

namespace DownloaderV3.Decoders.DataDecoders;

public class HexToBigIntegerArray : DataDecoder
{
    public override void BuildFromData(string topicData)
    {
        var hexStringsArray = new HexStringChunkDecoder();
        hexStringsArray.BuildFromData(topicData);
        var data = hexStringsArray.DecodedData as string[];

        DecodedData = data!.Select(x => BigInteger.Parse(x, NumberStyles.HexNumber)).Skip(2)
            .ToArray();
    }
}