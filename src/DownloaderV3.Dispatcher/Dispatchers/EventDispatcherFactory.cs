using Microsoft.Extensions.Logging;

namespace DownloaderV3.Dispatcher.Dispatchers;

public class EventDispatcherFactory : IEventDispatcherFactory
{
    private readonly ILogger _logger;
    private readonly Dictionary<string, Func<IEventDispatcher>> _dispatchers;

    public EventDispatcherFactory(ILogger logger)
    {
        _logger = logger;

        _dispatchers = new Dictionary<string, Func<IEventDispatcher>>
        {
            { "SQS", () => new SqsDispatcher(_logger) },
            // { "SNS", () => new SnsDispatcher(_logger) }
        };
    }

    public IEventDispatcher? CreateDispatcher(string dispatchType)
    {
        if (_dispatchers.TryGetValue(dispatchType, out var createDispatcher))
            return createDispatcher();

        _logger.LogError($"No dispatcher found for DispatchType: {dispatchType}");
        return null;
    }
}