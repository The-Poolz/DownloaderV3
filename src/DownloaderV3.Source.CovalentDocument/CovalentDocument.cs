using Flurl.Http;
using Newtonsoft.Json;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentDocument.Helpers;
using DownloaderV3.Source.CovalentDocument.Resolvers;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument;

public sealed class CovalentDocument : BaseDocument<InputData>
{
    public CalculatedValueResolver CalculatedValueResolver { get; set; }
    public string Url { get; }

    public CovalentDocument(long pageNumber, DownloaderSettings downloaderSettings, IReadOnlyDictionary<long, long> lastBlockDictionary, IReadOnlyDictionary<long, ChainInfo> chainSettings)
    {
        CalculatedValueResolver = new CalculatedValueResolver(lastBlockDictionary[downloaderSettings.ChainId], Convert.ToInt64(chainSettings[downloaderSettings.ChainId].BlockPerSecond * chainSettings[downloaderSettings.ChainId].DownloadTimeDelay), downloaderSettings);

        SourceLastBlock = lastBlockDictionary[downloaderSettings.ChainId];

        ValidateStartingBlock();
        SavedLastBlock = CalculatedValueResolver.EndingBlock;

        Url = UrlManager.SetupUrlParser(pageNumber, downloaderSettings, SavedLastBlock);
        FetchData();
    }

    public override string GetResponse() => Url.GetStringAsync().GetAwaiter().GetResult();

    public override InputData? DeserializeResponse(string sourceData) => JsonConvert.DeserializeObject<InputData>(sourceData);

    public void ValidateStartingBlock()
    {
        if (!CalculatedValueResolver.IsValidateStartingBlock)
            throw new InvalidOperationException($"Invalid starting block: {SourceLastBlock}");
    }
}