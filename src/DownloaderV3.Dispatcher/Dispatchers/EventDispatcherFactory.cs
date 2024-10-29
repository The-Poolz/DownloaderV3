using Microsoft.Extensions.Logging;

namespace DownloaderV3.Dispatcher.Dispatchers;

public class EventDispatcherFactory : IEventDispatcherFactory
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Type> _dispatcherTypes;

    public EventDispatcherFactory(ILogger logger, IServiceProvider serviceProvider, IEnumerable<IEventDispatcher> dispatchers)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        _dispatcherTypes = dispatchers.ToDictionary(
            dispatcher => dispatcher.GetType().Name.Replace("Dispatcher", "").ToUpperInvariant(),
            dispatcher => dispatcher.GetType()
        );
    }

    public IEventDispatcher? CreateDispatcher(string dispatchType)
    {
        if (_dispatcherTypes.TryGetValue(dispatchType, out var dispatcherType))
        {
            return _serviceProvider.GetService(dispatcherType) as IEventDispatcher;
        }

        _logger.LogError($"No dispatcher configured for DispatchType: {dispatchType}");
        return null;
    }
}