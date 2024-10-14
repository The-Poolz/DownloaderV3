namespace DownloaderV3.Destination.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PropertySetterAttribute(string propertyName, string propertyValue, string dbSetName)
    : Attribute
{
    public string PropertyName { get; } = propertyName;
    public string PropertyValue { get; } = propertyValue;
    public string DbSetName { get; } = dbSetName;
}