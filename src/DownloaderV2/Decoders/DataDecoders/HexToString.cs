namespace DownloaderV2.Decoders.DataDecoders;

public class HexToString : DataDecoder
{
    public override void BuildFromData(string topicData)
    {
        DecodedData = topicData;
    }
}