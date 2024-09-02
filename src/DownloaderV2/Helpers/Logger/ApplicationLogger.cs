namespace DownloaderV2.Helpers.Logger;

public static class ApplicationLogger
{
    private static ILogger? _logger;

    public static void Initialize(ILogger logger) => _logger = logger;

    public static void LogAndThrow(Exception ex, string additionalMessage = "")
    {
        _logger?.LogCritical($"{ex} {additionalMessage} Exception: {ex.Message} StackTrace: {ex.StackTrace}");
        throw ex;
    }

    public static dynamic LogAndThrowDynamic(Exception ex, string additionalMessage = "")
    {
        LogAndThrow(ex, additionalMessage);
        return default!;
    }

    public static void Log(string message) => _logger?.Log(message);
}