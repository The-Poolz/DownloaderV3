using UrlFiller.Resolver;

namespace DownloaderV3.Builders.LogBuilder.Resolvers;

public class FunctionCallValueResolver(Func<string, string> function) : IValueResolver
{
    public string GetValue(string input)
    {
        return function(input);
    }
}