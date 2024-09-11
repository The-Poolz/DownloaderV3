using UrlFiller;
using UrlFiller.Resolver;
using DownloaderV3.Helpers;
using DownloaderV3.HttpFlurlClient;
using DownloaderV3.Models.Covalent;
using DownloaderV3.Destination.Models;
using DownloaderV3.Builders.LogBuilder.Resolvers;

namespace DownloaderV3.Builders.LogBuilder;

public class LogDownloader : CalculatedValueResolver
{
    public InputData DownloadedContractData { get; }
    private string UrlSet { get; set; }
    private URLParser Url { get; set; }
    public long LastSavedBlock { get; set; }
    public long ChainLastBlock { get; }

    public LogDownloader(long pageNumber, DownloaderSettings downloaderSettings, IReadOnlyDictionary<long, long> lastBlockDictionary, IReadOnlyDictionary<long, ChainInfo> chainSettings) : base(lastBlockDictionary[downloaderSettings.ChainId], Convert.ToInt64(chainSettings[downloaderSettings.ChainId].BlockPerSecond * chainSettings[downloaderSettings.ChainId].DownloadTimeDelay), downloaderSettings)
    {
        ChainLastBlock = lastBlockDictionary[downloaderSettings.ChainId];
        UrlSet = downloaderSettings.UrlSet;

        if (!IsValidateStartingBlock)
            ApplicationLogger.LogAndThrow(new InvalidOperationException(string.Format(ExceptionMessages.InvalidStartingBlockErrorMessage, ChainLastBlock)));
        LastSavedBlock = EndingBlock;

        var downloaderSettingsResolver = new PropertyGetValueResolver(downloaderSettings);
        var endingBlockResolver = new FunctionCallValueResolver(_ => LastSavedBlock.ToString());
        var number = new FunctionCallValueResolver(_ => pageNumber.ToString());

        var valueResolvers = new Dictionary<string, IValueResolver>
        {
            ["ChainId"] = downloaderSettingsResolver,
            ["ContractAddress"] = downloaderSettingsResolver,
            ["StartingBlock"] = downloaderSettingsResolver,
            ["EndingBlock"] = endingBlockResolver,
            ["PageNumber"] = number,
            ["MaxPageNumber"] = downloaderSettingsResolver,
            ["Key"] = downloaderSettingsResolver
        };

        Url = new URLParser(valueResolvers, true);

        var response = Request.CovalentResponse(Url.ParseUrl(UrlSet)).GetAwaiter().GetResult();

        DownloadedContractData = response?.ToObject<InputData>() ?? ApplicationLogger.LogAndThrowDynamic(new NullReferenceException(ExceptionMessages.FailedToRetrieveDataFromCovalentApi));
    }
}