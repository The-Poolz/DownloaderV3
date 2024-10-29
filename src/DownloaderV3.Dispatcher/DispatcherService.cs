using DownloaderV3.DataBase;
using DownloaderV3.Dispatcher.Dispatchers;
using DownloaderV3.Dispatcher.Models;
using DownloaderV3.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DownloaderV3.Dispatcher
{
    public class DispatcherService( DownloaderV3Context context, ILogger logger, IEventDispatcherFactory? dispatcherFactory = null)
    {
        // TODO: change to DownloaderV3.DataBase after update DownloaderV3.DataBase version
        private readonly LocalContextWrapper _contextWrapper = new(context);
        private readonly IEventDispatcherFactory _dispatcherFactory = dispatcherFactory ?? CreateDefaultDispatcherFactory(logger);

        private static IEventDispatcherFactory CreateDefaultDispatcherFactory(ILogger logger)
        {
            var defaultDispatchers = new List<IEventDispatcher> { new SqsDispatcher(logger) };
            return new EventDispatcherFactory(logger, null!, defaultDispatchers);
        }

        public async Task SendDispatchAsync(IEnumerable<ResultObject> resultObjects)
        {
            var dispatchSettings = await _contextWrapper.DispatchSettings.Object.Where(ds => ds.IsActive)
                .ToListAsync();

            var tasks = resultObjects
                .Where(result => result.Count > 0)
                .Select(result => new
                {
                    Result = result,
                    Settings = dispatchSettings.FirstOrDefault(ds => ds.ChainId == result.ChainId && ds.ResponseType == result.EventName)
                })
                .Where(item => item.Settings != null)
                .Select(item => SendDispatchResponse(item.Result, item.Settings!)) 
                .ToList();

            await Task.WhenAll(tasks);
        }

        private async Task SendDispatchResponse(ResultObject result, DispatcherSettings settings)
        {
            try
            {
                var dispatcher = _dispatcherFactory.CreateDispatcher(settings.DispatchType);

                if (dispatcher == null)
                {
                    logger.LogError($"Unsupported DispatchType: {settings.DispatchType} for ChainId: {result.ChainId}");
                    return;
                }

                await dispatcher.DispatchAsync(result, settings);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to dispatch response for ChainId: {result.ChainId}, EventName: {result.EventName}");
            }
        }
    }
}
