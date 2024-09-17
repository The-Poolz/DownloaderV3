using UrlFiller.Resolver;

namespace DownloaderV3.Source.CovalentDocument.Resolvers;


//TODO : delete this class
public class FunctionCallValueResolver(Func<string, string> function) : IValueResolver
{
    public string GetValue(string input)
    {
        return function(input);
    }
}