namespace DownloaderV2.LogRouter;

public interface IBeforeSave
{
    public void Run(object obj);
}