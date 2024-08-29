using DownloaderV2.Result;

namespace DownloaderV2.Base;

public interface IDownloadHandler
{
    Task<IEnumerable<ResultObject>> HandleAsync();
}