namespace DownloaderV3.Dispatcher.Models;

// TODO: Delete all this folder Models after update DownloaderV3.DataBase version
public class DispatcherSettings
{
    public int ChainId { get; set; }
    public string ResponseType { get; set; } = null!;
    public string DispatchType { get; set; } = null!;
    public string DispatchEnvironment { get; set; } = null!;
    public string TriggerParameters { get; set; } = null!;
    public bool IsActive { get; set; }
}