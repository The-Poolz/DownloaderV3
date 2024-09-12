using System.Text;

namespace DownloaderV3.Decoders.DataDecoders;

public class ChunckHexToString : DataDecoder
{
    public override void BuildFromData(string topicData)
    {
        DecodedData = Encoding.UTF8.GetString(Enumerable.Range(0, topicData.Length / 2)
            .Select(x => Convert.ToByte(topicData.Substring(x * 2, 2), 16))
            .ToArray());
    }
}