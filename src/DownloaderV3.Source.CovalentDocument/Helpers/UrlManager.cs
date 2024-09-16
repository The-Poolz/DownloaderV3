using HandlebarsDotNet;
using DownloaderV3.Destination.Models;

namespace DownloaderV3.Source.CovalentDocument.Helpers;

public class UrlManager
{
    public static string SetupUrlParser(long pageNumber, DownloaderSettings downloaderSettings, long savedLastBlock)
    {
        var urlTemplate = downloaderSettings.UrlSet;

        var handlebars = Handlebars.Create();
        var template = handlebars.Compile(urlTemplate);

        var url = template(new
        {
            downloaderSettings.ChainId,
            downloaderSettings.ContractAddress,
            downloaderSettings.StartingBlock,
            EndingBlock = savedLastBlock,
            PageNumber = pageNumber,
            downloaderSettings.MaxPageNumber,
            downloaderSettings.Key
        });

        return url;
    }
}