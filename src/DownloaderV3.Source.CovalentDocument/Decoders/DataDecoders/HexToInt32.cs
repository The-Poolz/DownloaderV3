using Nethereum.Hex.HexTypes;

namespace DownloaderV3.Source.CovalentDocument.Decoders.DataDecoders;

public class HexToInt32 : DataDecoder
{
    public override void BuildFromData(string topicData)
    {
        DecodedData = Convert.ToInt32(new HexUTF8String(topicData).Value, 16);
    }
}