using DownloaderV2.Decoders;
using DownloaderV2.Extensions;
using DownloaderContext.Models;
using DownloaderV2.Models.Covalent;
using static DownloaderV2.LogRouter.LogRouter;

namespace DownloaderV2.Builders.LogBuilder;

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