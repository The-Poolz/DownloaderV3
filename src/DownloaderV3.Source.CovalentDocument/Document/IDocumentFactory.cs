using DownloaderV3.Destination.Models;

namespace DownloaderV3.Source.CovalentDocument.Document;

public interface IDocumentFactory
{
    BaseDocument<TData> Create<TData>(long pageNumber, DownloaderSettings settings, IReadOnlyDictionary<long, long> lastBlockDictionary, IReadOnlyDictionary<long, ChainInfo> chainSettings);
}