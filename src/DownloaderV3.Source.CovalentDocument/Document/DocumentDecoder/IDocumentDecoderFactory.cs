using DownloaderV3.Destination.Models;

namespace DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;

public interface IDocumentDecoderFactory
{
    BaseDocumentDecoder<TData> Create<TData>(DownloaderSettings settings, TData inputData) where TData : class;
}