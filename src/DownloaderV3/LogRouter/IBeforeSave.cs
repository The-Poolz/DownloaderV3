namespace DownloaderV3.LogRouter;

public interface IBeforeSave
{
    public void Run(object obj);
}