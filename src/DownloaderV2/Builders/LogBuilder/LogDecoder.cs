using DownloaderV2.Decoders;
using DownloaderV2.LogRouter;
using DownloaderContext.Types;
using DownloaderV2.Extensions;
using DownloaderContext.Models;
using DownloaderV2.Models.ApiCovalent;

namespace DownloaderV2.Builders.LogBuilder;

public class LogDecoder
{
    public SavedLogResponse LogResponses { get; }
    public int EventCount => LogResponses.Count;

    public LogDecoder(DownloaderSettings downloaderSettings, IInputData inputData)
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

    private ILogResponse GetLogType(ResponseType responseType, Dictionary<string, DataDecoder>[] dictionaries)
    {
        throw new NotImplementedException();
    }
}