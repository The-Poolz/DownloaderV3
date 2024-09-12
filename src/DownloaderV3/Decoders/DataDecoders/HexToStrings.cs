namespace DownloaderV3.Decoders.DataDecoders;

public class HexToStrings : DataDecoder
{
    private const int HexBase = 16;

    public override void BuildFromData(string topicData)
    {
        var hexStringsArray = new HexStringChunkDecoder();
        hexStringsArray.BuildFromData(topicData);
        var data = hexStringsArray.DecodedData as string[];

        if (data == null || data.Length < 3) 
            throw new InvalidOperationException("Decoded data is not a string array.");
        var hexStrings = new Queue<string>(data.Skip(2));
        DecodedData = new List<string>();

        while (hexStrings.Count > 0)
        {
            var nextStringLength = Convert.ToInt32(hexStrings.Dequeue(), HexBase);
            DecodedData.Add(ExtractString(nextStringLength, hexStrings));
        }
    }

    internal static string ExtractString(int length, Queue<string> hexStrings)
    {
        var currentString = string.Empty;
        var remainingLength = length;

        while (remainingLength > 0 && hexStrings.Count > 0)
        {
            var chunk = hexStrings.Dequeue();
            var hexToString = new ChunckHexToString();
            hexToString.BuildFromData(chunk);

            var takeLength = Math.Min(remainingLength, hexToString.DecodedData.Length);

            currentString += hexToString.DecodedData.Substring(0, takeLength);
            remainingLength -= takeLength;
        }

        return currentString;
    }
}
