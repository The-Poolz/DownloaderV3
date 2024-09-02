namespace DownloaderV2.Helpers.Logger;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"INFO: {message}");
    }

    public void LogCritical(string message)
    {
        Console.WriteLine($"CRITICAL: {message}");
    }
}