using System.Globalization;

namespace DownloaderV2.Decoders.DataDecoders;

public class StringToDateTime : DataDecoder
{
    public override void BuildFromData(string topicData)
    {
        string[] formats = { "dd.MM.yyyy H:mm:ss", "MM/dd/yyyy H:mm:ss", "M/d/yyyy h:mm:ss tt" };

        DecodedData = DateTime.ParseExact(topicData, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
    }
}