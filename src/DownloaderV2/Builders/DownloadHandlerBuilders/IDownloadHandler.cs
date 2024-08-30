using DownloaderV2.Result;

namespace DownloaderV2.Builders.DownloadHandlerBuilders;

public interface IDownloadHandler
{
    Task<IEnumerable<ResultObject>> HandleAsync();
}