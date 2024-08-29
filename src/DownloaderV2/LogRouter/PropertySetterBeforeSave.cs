namespace DownloaderV2.LogRouter;

public class PropertySetterBeforeSave(string propertyName, string propertyValue, string dbSetName)
    : IBeforeSave
{
    private string Value { get; } = propertyValue;
    private string Name { get; } = propertyName;
    public string DbSetName { get; } = dbSetName;

    public void Run(object obj)
    {
       var propertyInfo = obj.GetType().GetProperty(Name);
       propertyInfo?.SetValue(obj, Value);
    }
}