using DownloaderV3.DataBase.Models;
using DownloaderV3.Dispatcher.Models;
using DownloaderV3.Result;

namespace DownloaderV3.Dispatcher.Dispatchers;

public interface IEventDispatcher
{
    Task DispatchAsync(ResultObject result, DispatcherSettings? settings);
}