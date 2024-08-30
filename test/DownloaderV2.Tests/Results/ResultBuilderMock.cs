using DownloaderV2.Result;
using DownloaderV2.Tests.Results.DbResults;

namespace DownloaderV2.Tests.Results;

public static class ResultBuilderMock
{
    public static readonly List<ResultObject> ResultList =
    [
        new ResultObject
        {
            ChainId = DownloaderSettingsResult.SwapBNBParty1.ChainId,
            EventName = "SwapBNBParty",
            Count = 10
        },

        new ResultObject
        {
            ChainId = DownloaderSettingsResult.SwapBNBParty2.ChainId,
            EventName = "SwapBNBParty",
            Count = 1
        }
    ];

    public static readonly Dictionary<long, ResultObject> ResultSettings = new()
    {
        { DownloaderSettingsResult.SwapBNBParty1.ChainId, ResultList[0] }
    };
}