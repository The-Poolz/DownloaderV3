using Flurl.Http;
using SourceLastBlock.Helpers;

namespace SourceLastBlock.HttpFlurlClient;

public static class Request
{
    public static async Task<string?> CovalentResponse(string url)
    {
        try
        {
            return await url.GetStringAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"ERROR: {ExceptionMessages.GeneralCovalentDataRetrievalError} - {e.Message}");
            throw;
        }
    }
}