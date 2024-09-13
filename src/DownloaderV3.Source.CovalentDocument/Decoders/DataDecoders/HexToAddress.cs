namespace DownloaderV3.Source.CovalentDocument.Decoders.DataDecoders;

public class HexToAddress : DataDecoder
{
    public override void BuildFromData(string topicData)
    {
        DecodedData = $"0x{topicData[^40..]}";
    }
}