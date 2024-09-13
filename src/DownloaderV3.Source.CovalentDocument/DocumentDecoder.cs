using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Decoders;
using static DownloaderV3.Source.CovalentDocument.DocumentRouter.DocumentRouter;
using DownloaderV3.Source.CovalentDocument.Extensions;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument;

public class DocumentDecoder
{
    public SavedDocumentResponse DocumentResponses { get; }
    public int EventCount => DocumentResponses.Count;

    public DocumentDecoder(DownloaderSettings downloaderSettings, InputData inputData)
    {
        DocumentResponses = new SavedDocumentResponse();

        if (inputData.Data.Items.Length == 0) return;

        foreach (var item in inputData.Data.Items.Where(T => T.RawLogTopics[0] == downloaderSettings.EventHash))
        {
            var data = inputData.Data.Clone();
            data.Items = new[] { item };

            var topicDecoder = new TopicDecoder(data, downloaderSettings.DownloaderMappings).LogData;

            DocumentResponses.Add(GetDocumentType(downloaderSettings.ResponseType, new[] { topicDecoder }));
        }
    }
}