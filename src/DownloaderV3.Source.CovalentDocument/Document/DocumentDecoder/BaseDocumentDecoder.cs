using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Extensions;

namespace DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;

public abstract class BaseDocumentDecoder<TData>
{
    public virtual SavedDocumentResponse DocumentResponses { get; set; } = null!;
    public int EventCount => DocumentResponses.Count;

    public abstract SavedDocumentResponse DecodeDocument(DownloaderSettings settings, TData inputData);
}