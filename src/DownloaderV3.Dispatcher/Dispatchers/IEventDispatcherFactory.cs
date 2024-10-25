namespace DownloaderV3.Dispatcher.Dispatchers;

public interface IEventDispatcherFactory
{
    IEventDispatcher? CreateDispatcher(string dispatchType);
}