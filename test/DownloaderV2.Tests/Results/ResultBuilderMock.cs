﻿using DownloaderV2.Result;
using DownloaderV2.Tests.Results.DbResults;

namespace DownloaderV2.Tests.Results;

public static class ResultBuilderMock
{
    public static readonly List<ResultObject> ResultList =
    [
        new ResultObject
        {
            ChainId = DownloaderSettingsResult.SomeEvent.ChainId,
            EventName = "SomeEvent",
            Count = 10
        },
        new ResultObject
        {
            ChainId = DownloaderSettingsResult.SomeAnotherEvent.ChainId,
            EventName = "SomeAnotherEvent",
            Count = 1
        }
    ];

    public static readonly Dictionary<long, ResultObject> ResultSettings = new()
    {
        { DownloaderSettingsResult.SomeEvent.ChainId, ResultList[0] }
    };
}