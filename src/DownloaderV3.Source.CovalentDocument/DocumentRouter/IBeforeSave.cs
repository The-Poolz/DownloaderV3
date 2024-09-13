namespace DownloaderV3.Source.CovalentDocument.DocumentRouter;

public interface IBeforeSave
{
    public void Run(object obj);
}