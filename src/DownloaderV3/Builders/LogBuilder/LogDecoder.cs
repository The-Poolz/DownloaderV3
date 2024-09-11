using DownloaderV3.Decoders;
using DownloaderV3.Extensions;
using DownloaderV3.Models.Covalent;
using DownloaderV3.Destination.Models;
using static DownloaderV3.LogRouter.LogRouter;

namespace DownloaderV3.Builders.LogBuilder;

public class LogDecoder
{
    public SavedLogResponse LogResponses { get; }
    public int EventCount => LogResponses.Count;

    public LogDecoder(DownloaderSettings downloaderSettings, InputData inputData)
    {
        LogResponses = new SavedLogResponse();

        if (inputData.Data.Items.Length == 0) return;

        foreach (var item in inputData.Data.Items.Where(T => T.RawLogTopics[0] == downloaderSettings.EventHash))
        {
            var data = inputData.Data.Clone();
            data.Items = new[] { item };

            var topicDecoder = new TopicDecoder(data, downloaderSettings.DownloaderMappings).LogData;

            LogResponses.Add(GetLogType(downloaderSettings.ResponseType, new[] { topicDecoder }));
        }
    }
}