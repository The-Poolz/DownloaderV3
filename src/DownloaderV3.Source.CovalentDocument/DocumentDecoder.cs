using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Decoders;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentDocument.Extensions;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;
using static DownloaderV3.Source.CovalentDocument.DocumentRouter.DocumentRouter;

namespace DownloaderV3.Source.CovalentDocument;

public class DocumentDecoder : BaseDocumentDecoder<InputData>
{
    public DocumentDecoder(DownloaderSettings downloaderSettings, InputData inputData)
    {
        this.DocumentResponses = [];
        DecodeDocument(downloaderSettings, inputData);
    }

    public sealed override SavedDocumentResponse DecodeDocument(DownloaderSettings downloaderSettings, InputData inputData)
    {
        if (inputData.Data.Items.Length == 0) return DocumentResponses;

        foreach (var item in inputData.Data.Items.Where(T => T.RawLogTopics[0] == downloaderSettings.EventHash))
        {
            var data = inputData.Data.Clone();
            data.Items = new[] { item };

            var topicDecoder = new TopicDecoder(data, downloaderSettings.DownloaderMappings).LogData;

            DocumentResponses.Add(GetDocumentType(downloaderSettings.ResponseType, new[] { topicDecoder }));
        }

        return DocumentResponses;
    }
}