using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Extensions;

namespace DownloaderV3.Source.CovalentDocument.Document;

public abstract class BaseDocumentDecoder<TData>
{
    public SavedDocumentResponse DocumentResponses { get; set; } = null!;
    public int EventCount => DocumentResponses.Count;

    public abstract SavedDocumentResponse DecodeDocument(DownloaderSettings settings, TData inputData);
}