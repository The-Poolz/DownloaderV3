using DownloaderV3.Result;
using DownloaderV3.Helpers;
using Net.Utils.TaskManager;
using DownloaderV3.Destination;
using DownloaderV3.Destination.Models;
using DownloaderV3.Builders.LastBlockBuilder;
using Microsoft.Extensions.DependencyInjection;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentLastBlock.SourcePage;
using DownloaderV3.Source.CovalentDocument.DocumentRouter;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;
using DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;
using Microsoft.Extensions.Logging;

namespace DownloaderV3;

public class DownloadHandler<TData> where TData : InputData, IHasPagination
{
    private readonly BaseDestination _destination;
    private readonly GetSourcePage _getSourcePage;
    private readonly SettingDownloader _settingDownloader;
    private readonly SqlQueryHelper _sqlQueryHelper;
    private readonly ResultBuilder _resultBuilder = new();
    private readonly IDocumentFactory _documentFactory;
    private readonly IDocumentDecoderFactory _documentDecoderFactory;

    private IReadOnlyDictionary<long, long> _lastBlockDictionary = new Dictionary<long, long>();

    public DownloadHandler(IServiceProvider serviceProvider)
    {
        _getSourcePage = serviceProvider.GetRequiredService<GetSourcePage>();
        _destination = serviceProvider.GetRequiredService<BaseDestination>();

        _settingDownloader = new SettingDownloader(_destination);
        _sqlQueryHelper = new SqlQueryHelper(_destination);

        _documentFactory = serviceProvider.GetRequiredService<IDocumentFactory>();
        _documentDecoderFactory = serviceProvider.GetRequiredService<IDocumentDecoderFactory>();
    }

    public DownloadHandler(BaseDestination destination, Action<ILoggingBuilder>? loggingConfiguration = null)
    {
        _destination = destination;

        var serviceCollection = ServiceConfigurator.BuildServiceProvider(destination, loggingConfiguration);

        _getSourcePage = serviceCollection.GetRequiredService<GetSourcePage>();
        _documentFactory = serviceCollection.GetRequiredService<IDocumentFactory>();
        _documentDecoderFactory = serviceCollection.GetRequiredService<IDocumentDecoderFactory>();

        _settingDownloader = new SettingDownloader(_destination);
        _sqlQueryHelper = new SqlQueryHelper(_destination);
    }

    public async Task<IEnumerable<ResultObject>> HandleAsync()
    {
        DocumentRouter.Initialize(_destination.GetType());

        _lastBlockDictionary = new LastBlockSource(_getSourcePage).LastBlockDictionary;

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
        var document = _documentFactory.Create<TData>(pageNumber, contractSettings, _lastBlockDictionary, _settingDownloader.ChainSettings);
        HandleTopics(contractSettings, document);
        if (document.DownloadedContractData!.GetPagination().HasMore)
        {
            taskManager.AddTask(HandleContracts(pageNumber + 1, contractSettings, taskManager));
        }
    });

    private void HandleTopics(BaseDownloaderSettings contractSettings, BaseDocument<TData> document)
    {
        var uniqueTopics = _settingDownloader.DownloaderSettings
            .Where(x => x.ChainId == contractSettings.ChainId && x.ContractAddress == contractSettings.ContractAddress && x.EventHash == contractSettings.EventHash)
            .ToList();

        Parallel.ForEach(uniqueTopics, (topicSettings, _) =>
        {
            HandleTopicSaving(topicSettings, document);
        });
    }

    private void HandleTopicSaving(DownloaderSettings topicSettings, BaseDocument<TData> document)
    {
        var documentDecoder = _documentDecoderFactory.Create(topicSettings, document.DownloadedContractData!);

        documentDecoder.DocumentResponses.LockedSaveAll(_destination);

        if (!document.DownloadedContractData!.GetPagination().HasMore)
        {
            UpdateDownloaderSettings(topicSettings, document);
            AddResult(topicSettings, documentDecoder.EventCount);
        }
    }

    private void UpdateDownloaderSettings(DownloaderSettings topicSettings, BaseDocument<TData> document)
    {
        _sqlQueryHelper.UpdateDownloaderSettings(topicSettings, document.SavedLastBlock, document.SourceLastBlock);
    }

    private void AddResult(DownloaderSettings topicSettings, int eventCount)
    {
        _resultBuilder.AddResult(new ResultObject().SetSuccess(topicSettings, eventCount));
    }
}