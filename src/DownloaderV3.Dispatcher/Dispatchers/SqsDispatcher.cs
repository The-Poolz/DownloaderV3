using Amazon.SQS;
using Newtonsoft.Json;
using Amazon.SQS.Model;
using DownloaderV3.Result;
using EnvironmentManager.Core;
using Microsoft.Extensions.Logging;
using DownloaderV3.DataBase.Models;

namespace DownloaderV3.Dispatcher.Dispatchers;

public class SqsDispatcher(ILogger logger) : IEventDispatcher
{
    public readonly IAmazonSQS SqsClient = new AmazonSQSClient();

    public async Task DispatchAsync(ResultObject downloaderResult, DispatcherSettings? settings)
    {
        var sqsUrl = GetQueueUrl(settings!.DispatchEnvironment!);

        var serializedMessage = SerializeMessage(downloaderResult);

        await SendMessageToQueue(sqsUrl, serializedMessage);
    }

    public static string GetQueueUrl(string dispatchEnvironment)
    {
        var envManager = new EnvManager();
        return envManager.Get<string>(dispatchEnvironment, true);
    }

    private string SerializeMessage(ResultObject resultObject)
    {
        var message = new
        {
            resultObject.ChainId,
            resultObject.EventName,
            resultObject.Count,
            // resultObject.From,
            // resultObject.To,
        };
        return JsonConvert.SerializeObject(message, Formatting.Indented);
    }

    private async Task SendMessageToQueue(string queueUrl, string messageBody)
    {
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = messageBody
        };

        try
        {
            await SqsClient.SendMessageAsync(sendMessageRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending message to SQS queue: {QueueUrl}", queueUrl);
        }
    }
}