using DownloaderV3.Result;
using DownloaderV3.Helpers;
using Net.Utils.TaskManager;
using DownloaderV3.Destination;
using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument;
using DownloaderV3.Builders.LastBlockBuilder;
using DownloaderV3.Source.CovalentLastBlock.SourcePage;
using DownloaderV3.Source.CovalentDocument.DocumentRouter;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;
using DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;
using Microsoft.Extensions.DependencyInjection;

namespace DownloaderV3;

public class DownloadHandler
{
    private readonly GetSourcePage _sourcePage;
    private readonly BaseDestination _destination;
    private readonly SettingDownloader _settingDownloader;
    private readonly SqlQueryHelper _sqlQueryHelper;
    private readonly ResultBuilder _resultBuilder = new();
    private readonly IDocumentDecoderFactory _documentDecoderFactory;
    private IReadOnlyDictionary<long, long> _lastBlockDictionary = new Dictionary<long, long>();

    public DownloadHandler(IServiceProvider serviceProvider)
    {
        _documentDecoderFactory = serviceProvider.GetRequiredService<IDocumentDecoderFactory>();
        _destination = serviceProvider.GetRequiredService<BaseDestination>();
        _sourcePage = serviceProvider.GetRequiredService<GetSourcePage>();

        _settingDownloader = new SettingDownloader(_destination);
        _sqlQueryHelper = new SqlQueryHelper(_destination);
    }

    public async Task<IEnumerable<ResultObject>> HandleAsync()
    {
        DocumentRouter.Initialize(_destination.GetType());

        _lastBlockDictionary = new LastBlockSource(_sourcePage).LastBlockDictionary;

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
        if (downloader.DownloadedContractData!.Data.Pagination.HasMore)
        {
            taskManager.AddTask(HandleContracts(pageNumber + 1, contractSettings, taskManager));
        }
    });

    private void HandleTopics(BaseDownloaderSettings contractSettings, BaseDocument<InputData> downloader)
    {
        var uniqueTopics = _settingDownloader.DownloaderSettings
            .Where(x => x.ChainId == contractSettings.ChainId && x.ContractAddress == contractSettings.ContractAddress && x.EventHash == contractSettings.EventHash)
            .ToList();

        Parallel.ForEach(uniqueTopics, (topicSettings, _) =>
        {
            HandleTopicSaving(topicSettings, downloader);
        });
    }

    private void HandleTopicSaving(DownloaderSettings topicSettings, BaseDocument<InputData> downloader)
    {
        var documentDecoder = _documentDecoderFactory.Create(topicSettings, downloader.DownloadedContractData!);
        documentDecoder.DocumentResponses.LockedSaveAll(_destination);

        if (!downloader.DownloadedContractData!.Data.Pagination.HasMore)
        {
            UpdateDownloaderSettings(topicSettings, downloader);
            AddResult(topicSettings, documentDecoder.EventCount);
        }
    }

    private void UpdateDownloaderSettings(DownloaderSettings topicSettings, BaseDocument<InputData> downloader)
    {
        _sqlQueryHelper.UpdateDownloaderSettings(topicSettings, downloader.SavedLastBlock, downloader.SourceLastBlock);
    }

    private void AddResult(DownloaderSettings topicSettings, int eventCount)
    {
        _resultBuilder.AddResult(new ResultObject().SetSuccess(topicSettings, eventCount));
    }
}