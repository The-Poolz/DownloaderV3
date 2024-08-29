using Flurl.Http;
using DownloaderV2.Helpers;
using Newtonsoft.Json.Linq;

namespace DownloaderV2.HttpFlurlClient;

public abstract class Request
{
    public static async Task<JToken?> CovalentResponse(string url)
    {
        using var client = new FlurlClient(url);
        try
        {
            return JToken.Parse(await client.Request().SendAsync(new HttpMethod("GET")).ReceiveString());
        }
        catch (Exception e)
        {
            return ApplicationLogger.LogAndThrowDynamic(e, ExceptionMessages.GeneralCovalentDataRetrievalError);
        }
    }
}