namespace DownloaderV3.Helpers;

public static class ApplicationLogger
{
    public static void LogAndThrow(Exception ex, string additionalMessage = "")
    {
        Console.WriteLine($"CRITICAL: {ex} {additionalMessage} Exception: {ex.Message} StackTrace: {ex.StackTrace}");
        throw ex;
    }

    public static dynamic LogAndThrowDynamic(Exception ex, string additionalMessage = "")
    {
        LogAndThrow(ex, additionalMessage);
        return default!;
    }

    public static void Log(string message) => Console.WriteLine($"INFO: {message}");
}