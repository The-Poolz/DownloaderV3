using UrlFiller.Resolver;
using DownloaderV2.Helpers.Logger;

namespace DownloaderV2.Builders.LogBuilder.Resolvers;

public class PropertyGetValueResolver(object targetObject) : IValueResolver
{
    public string GetValue(string input)
    {
        var propertyInfo = targetObject.GetType().GetProperty(input);

        return propertyInfo == null
            ? ApplicationLogger.LogAndThrowDynamic(new ArgumentException($"No property '{input}' found in object of type '{targetObject.GetType().Name}'"))
            : propertyInfo.GetValue(targetObject)?.ToString() ?? string.Empty;
    }
}