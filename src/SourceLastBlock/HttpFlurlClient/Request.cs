using Flurl.Http;
using Newtonsoft.Json.Linq;
using SourceLastBlock.Helpers;

namespace SourceLastBlock.HttpFlurlClient;

public static class Request
{
    public static async Task<JToken?> CovalentResponse(string url)
    {
        try
        {
            return await url.GetJsonAsync<JToken>();
        }
        catch (Exception e)
        {
            return ApplicationLogger.LogAndThrowDynamic(e, ExceptionMessages.GeneralCovalentDataRetrievalError);
        }
    }
}