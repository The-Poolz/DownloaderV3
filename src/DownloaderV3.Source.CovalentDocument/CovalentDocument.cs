using UrlFiller;
using Flurl.Http;
using Newtonsoft.Json;
using UrlFiller.Resolver;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentDocument.Resolvers;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument;

public sealed class CovalentDocument : BaseDocument
{
    public CalculatedValueResolver CalculatedValueResolver { get; set; }
    private string UrlSet { get; set; }
    private URLParser Url { get; set; } = null!;
    public long SavedLastBlock { get; set; }
    public long SourceLastBlock { get; }

    public CovalentDocument(long pageNumber, DownloaderSettings downloaderSettings, IReadOnlyDictionary<long, long> lastBlockDictionary, IReadOnlyDictionary<long, ChainInfo> chainSettings)
    {
        CalculatedValueResolver = new CalculatedValueResolver(lastBlockDictionary[downloaderSettings.ChainId], Convert.ToInt64(chainSettings[downloaderSettings.ChainId].BlockPerSecond * chainSettings[downloaderSettings.ChainId].DownloadTimeDelay), downloaderSettings);

        SourceLastBlock = lastBlockDictionary[downloaderSettings.ChainId];
        UrlSet = downloaderSettings.UrlSet;

        ValidateStartingBlock();
        SavedLastBlock = CalculatedValueResolver.EndingBlock;

        SetupUrlParser(pageNumber, downloaderSettings);
        FetchData();
    }

    public override string? GetResponse() => Url.ParseUrl(UrlSet).GetStringAsync().GetAwaiter().GetResult();

    public override InputData? DeserializeResponse(string sourceData) => JsonConvert.DeserializeObject<InputData>(sourceData);

    public void ValidateStartingBlock()
    {
        if (!CalculatedValueResolver.IsValidateStartingBlock)
            throw new InvalidOperationException($"Invalid starting block: {SourceLastBlock}");
    }

    public void SetupUrlParser(long pageNumber, DownloaderSettings downloaderSettings)
    {
        var downloaderSettingsResolver = new PropertyGetValueResolver(downloaderSettings);
        var endingBlockResolver = new FunctionCallValueResolver(_ => SavedLastBlock.ToString());
        var number = new FunctionCallValueResolver(_ => pageNumber.ToString());

        var resolvers = new Dictionary<string, IValueResolver>
        {
            ["ChainId"] = downloaderSettingsResolver,
            ["ContractAddress"] = downloaderSettingsResolver,
            ["StartingBlock"] = downloaderSettingsResolver,
            ["EndingBlock"] = endingBlockResolver,
            ["PageNumber"] = number,
            ["MaxPageNumber"] = downloaderSettingsResolver,
            ["Key"] = downloaderSettingsResolver
        };

        Url = new URLParser(resolvers, true);
    }
}