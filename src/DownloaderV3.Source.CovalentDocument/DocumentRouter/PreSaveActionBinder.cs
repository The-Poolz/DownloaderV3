namespace DownloaderV3.Source.CovalentDocument.DocumentRouter;

public class PreSaveActionBinder(Type type, IBeforeSave? beforeSave = null)
{
    public Type TypeHolder { get; } = type;
    public IBeforeSave? BeforeSave { get; } = beforeSave;
}