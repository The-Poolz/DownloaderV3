using DownloaderContext;
using DownloaderV2.Result;
using DownloaderV2.Helpers;
using Net.Utils.TaskManager;
using DownloaderContext.Models;
using DownloaderV2.Builders.LogBuilder;
using DownloaderV2.Builders.LastBlockBuilder;
using DownloaderV2.Builders.LastBlockBuilder.SourcePage;

namespace DownloaderV2;

public class DownloadHandler(BaseDownloaderContext context)
{
    private readonly SettingDownloader _settingDownloader = new(context);
    private readonly SqlQueryHelper _sqlQueryHelper = new(context);
    private readonly ResultBuilder _resultBuilder = new();
    private IReadOnlyDictionary<long, long> _lastBlockDictionary = new Dictionary<long, long>();

    public async Task<IEnumerable<ResultObject>> HandleAsync()
    {
        LogRouter.LogRouter.Initialize(context.GetType());

        _lastBlockDictionary = await new LastBlockDownloader(new GetLastBlock()).LastBlockDictionary;
        var uniqueEvents = _settingDownloader.DownloaderSettings
            .GroupBy(x => new { x.ChainId, x.ContractAddress, x.EventHash })
            .Select(group => group.First())
            .ToList();

        var taskManager = new TaskManager();
        var tasks = uniqueEvents.Select(contractSettings => HandleContracts(0, contractSettings, taskManager));
        taskManager.AddRange(tasks);
        await taskManager.StartAsync();

        _sqlQueryHelper.TrySaveChangeAsync();
        _resultBuilder.PrintResult();
        return _resultBuilder.Result;
    }

    private Task HandleContracts(int pageNumber, DownloaderSettings contractSettings, ITaskManager taskManager) => new(() =>
    {
        var downloader = new LogDownloader(pageNumber, contractSettings, _lastBlockDictionary, _settingDownloader.ChainSettings);
        HandleTopics(contractSettings, downloader);
        if (downloader.DownloadedContractData.Data.Pagination.HasMore)
        {
            taskManager.AddTask(HandleContracts(pageNumber + 1, contractSettings, taskManager));
        }
    });

    private void HandleTopics(BaseDownloaderSettings contractSettings, LogDownloader downloader)
    {
        var uniqueTopics = _settingDownloader.DownloaderSettings
            .Where(x => x.ChainId == contractSettings.ChainId && x.ContractAddress == contractSettings.ContractAddress && x.EventHash == contractSettings.EventHash)
            .ToList();

        Parallel.ForEach(uniqueTopics, (topicSettings, _) =>
        {
            HandleTopicSaving(topicSettings, downloader);
        });
    }

    private void HandleTopicSaving(DownloaderSettings topicSettings, LogDownloader downloader)
    {
        var logDecoder = new LogDecoder(topicSettings, downloader.DownloadedContractData);
        logDecoder.LogResponses.LockedSaveAll(context);

        if (!downloader.DownloadedContractData.Data.Pagination.HasMore)
        {
            UpdateDownloaderSettings(topicSettings, downloader);
            AddResult(topicSettings, logDecoder.EventCount);
        }
    }

    private void UpdateDownloaderSettings(DownloaderSettings topicSettings, LogDownloader downloader)
    {
        _sqlQueryHelper.UpdateDownloaderSettings(topicSettings, downloader.LastSavedBlock, downloader.ChainLastBlock);
    }

    private void AddResult(DownloaderSettings topicSettings, int eventCount)
    {
        _resultBuilder.AddResult(new ResultObject().SetSuccess(topicSettings, eventCount));
    }
}