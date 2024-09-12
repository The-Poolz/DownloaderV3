using UrlFiller.Resolver;

namespace DownloaderV3.Source.CovalentDocument.Resolvers;

public class PropertyGetValueResolver(object targetObject) : IValueResolver
{
    public string GetValue(string input)
    {
        var propertyInfo = targetObject.GetType().GetProperty(input);

        return propertyInfo == null
            ? throw new ArgumentException($"No property '{input}' found in object of type '{targetObject.GetType().Name}'")
            : propertyInfo.GetValue(targetObject)?.ToString() ?? string.Empty;
    }
}