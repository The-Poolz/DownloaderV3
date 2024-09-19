using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument.Document;

public class DocumentFactory : IDocumentFactory
{
    public BaseDocument<TData> Create<TData>(long pageNumber, DownloaderSettings settings, IReadOnlyDictionary<long, long> lastBlockDictionary, IReadOnlyDictionary<long, ChainInfo> chainSettings)
    {
        if (typeof(TData) == typeof(InputData))
        {
            return (new CovalentDocument(pageNumber, settings, lastBlockDictionary, chainSettings) as BaseDocument<TData>)!;
        }

        throw new NotSupportedException($"Downloader for type {typeof(TData).Name} is not supported.");
    }
}