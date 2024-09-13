using DownloaderV3.Result;
using DownloaderV3.Helpers;
using Net.Utils.TaskManager;
using DownloaderV3.Destination;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument;
using DownloaderV3.Builders.LastBlockBuilder;
using DownloaderV3.Source.CovalentLastBlock.SourcePage;
using DownloaderV3.Source.CovalentDocument.DocumentRouter;

namespace DownloaderV3;

public class DownloadHandler(BaseDestination destination, GetSourcePage sourcePage)
{
    private readonly SettingDownloader _settingDownloader = new(destination);
    private readonly SqlQueryHelper _sqlQueryHelper = new(destination);
    private readonly ResultBuilder _resultBuilder = new();
    private IReadOnlyDictionary<long, long> _lastBlockDictionary = new Dictionary<long, long>();

    public async Task<IEnumerable<ResultObject>> HandleAsync()
    {
        // TODO: From ServiceProvider in the next step
        DocumentRouter.Initialize(destination.GetType());

        _lastBlockDictionary = new LastBlockSource(sourcePage).LastBlockDictionary;

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
        var downloader = new CovalentDocument(pageNumber, contractSettings, _lastBlockDictionary, _settingDownloader.ChainSettings);
        HandleTopics(contractSettings, downloader);
        if (downloader.DownloadedContractData.Data.Pagination.HasMore)
        {
            taskManager.AddTask(HandleContracts(pageNumber + 1, contractSettings, taskManager));
        }
    });

    private void HandleTopics(BaseDownloaderSettings contractSettings, CovalentDocument downloader)
    {
        var uniqueTopics = _settingDownloader.DownloaderSettings
            .Where(x => x.ChainId == contractSettings.ChainId && x.ContractAddress == contractSettings.ContractAddress && x.EventHash == contractSettings.EventHash)
            .ToList();

        Parallel.ForEach(uniqueTopics, (topicSettings, _) =>
        {
            HandleTopicSaving(topicSettings, downloader);
        });
    }

    private void HandleTopicSaving(DownloaderSettings topicSettings, CovalentDocument downloader)
    {
        var documentDecoder = new DocumentDecoder(topicSettings, downloader.DownloadedContractData);
        documentDecoder.DocumentResponses.LockedSaveAll(destination);

        if (!downloader.DownloadedContractData.Data.Pagination.HasMore)
        {
            UpdateDownloaderSettings(topicSettings, downloader);
            AddResult(topicSettings, documentDecoder.EventCount);
        }
    }

    private void UpdateDownloaderSettings(DownloaderSettings topicSettings, CovalentDocument downloader)
    {
        _sqlQueryHelper.UpdateDownloaderSettings(topicSettings, downloader.SavedLastBlock, downloader.SourceLastBlock);
    }

    private void AddResult(DownloaderSettings topicSettings, int eventCount)
    {
        _resultBuilder.AddResult(new ResultObject().SetSuccess(topicSettings, eventCount));
    }
}