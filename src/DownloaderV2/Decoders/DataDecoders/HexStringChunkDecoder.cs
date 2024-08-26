namespace DownloaderV2.Decoders.DataDecoders;

public class HexStringChunkDecoder : DataDecoder
{
    public override void BuildFromData(string topicData)
    {
        if (string.IsNullOrEmpty(topicData) || !topicData.StartsWith("0x"))
        {
            throw new ArgumentException("Invalid hex string.");
        }

        topicData = topicData[2..];
        DecodedData = Enumerable.Range(0, topicData.Length / 64)
            .Select(i => topicData.Substring(i * 64, 64))
            .ToArray();
    }
}