using System.Globalization;
using Nethereum.Hex.HexTypes;

namespace DownloaderV2.Decoders.DataDecoders;

public class TimestampToDateTime : DataDecoder
{
    const long MaxTimestamp = 253402300799;
    public override void BuildFromData(string topicData)
    {
        if (!long.TryParse(new HexUTF8String(topicData).Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var timestamp))
            timestamp = MaxTimestamp;

        DecodedData = DateTimeOffset.FromUnixTimeSeconds(Math.Min(timestamp, MaxTimestamp)).DateTime;
    }
}