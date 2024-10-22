using DownloaderV3.DataBase;
using DownloaderV3.DataBase.Models;
using DownloaderV3.Dispatcher.Dispatchers;
using DownloaderV3.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DownloaderV3.Dispatcher
{
    public class DispatcherService(DownloaderV3Context context, ILogger logger)
    {
        public async Task SendDispatchAsync(IEnumerable<ResultObject> resultObjects)
        {
            var dispatchSettings = await context.DispatcherSettings.Where(ds => ds.IsActive)
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
                logger.LogInformation($"Dispatching response for ChainId: {result.ChainId}, EventName: {result.EventName}");

                //TODO: Implement SNS dispatcher + make a dynamic option to add a new implementation
                IEventDispatcher dispatcher = (settings.DispatchType switch
                {
                    "SQS" => new SqsDispatcher(logger),
                    //"SNS" => new SnsDispatcher(_snsClient, logger),
                    _ => null
                })!;

                await dispatcher.DispatchAsync(result, settings);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to dispatch response for ChainId: {result.ChainId}, EventName: {result.EventName}");
            }
        }
    }
}
