namespace DownloaderV2.LogRouter;

public class PreSaveActionBinder(Type type, IBeforeSave? beforeSave = null)
{
    public Type TypeHolder { get; } = type;
    public IBeforeSave? BeforeSave { get; } = beforeSave;
}