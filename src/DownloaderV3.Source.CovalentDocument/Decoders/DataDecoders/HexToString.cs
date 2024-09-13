namespace DownloaderV3.Source.CovalentDocument.Decoders.DataDecoders;

public class HexToString : DataDecoder
{
    public override void BuildFromData(string topicData)
    {
        DecodedData = topicData;
    }
}