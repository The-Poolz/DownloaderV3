using Flurl.Http;
using SourceLastBlock.Helpers;

namespace SourceLastBlock.HttpFlurlClient;

public class LoggedRequest
{
    public static async Task<string?> GetStringAsyncWithLogger(string url)
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