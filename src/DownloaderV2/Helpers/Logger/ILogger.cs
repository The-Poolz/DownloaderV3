namespace DownloaderV2.Helpers.Logger;

public interface ILogger
{
    void Log(string message);
    void LogCritical(string message);
}