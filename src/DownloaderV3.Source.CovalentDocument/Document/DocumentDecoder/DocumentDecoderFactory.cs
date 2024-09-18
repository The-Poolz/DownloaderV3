using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;

public class DocumentDecoderFactory : IDocumentDecoderFactory
{
    public BaseDocumentDecoder<TData> Create<TData>(DownloaderSettings settings, TData inputData) where TData : class
    {
        if (typeof(TData) == typeof(InputData))
        {
            return (new Source.CovalentDocument.DocumentDecoder(settings, (inputData as InputData)!) as BaseDocumentDecoder<TData>)!;
        }

        throw new NotSupportedException($"Decoder for type {typeof(TData).Name} is not supported.");
    }
}