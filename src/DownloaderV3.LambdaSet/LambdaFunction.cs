using Amazon.Lambda.Core;
using DownloaderV3.Result;
using DownloaderV3.Helpers;
using DownloaderV3.DataBase;
using DownloaderV3.Destination;
using Microsoft.Extensions.Logging;
using ConfiguredSqlConnection.Extensions;
using DownloaderV3.Source.CovalentLastBlock;
using Microsoft.Extensions.DependencyInjection;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentLastBlock.SourcePage;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;
using DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace DownloaderV3.LambdaSet;

public class LambdaFunction
{
    private readonly ServiceProvider _serviceProvider;

    public LambdaFunction()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);

        _serviceProvider = services.BuildServiceProvider();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(config =>
        {
            config.AddConsole();
        });

        services.AddTransient<GetSourcePage, GetLastBlockCovalent>();
        services.AddTransient<IDocumentFactory, DocumentFactory>();
        services.AddTransient<IDocumentDecoderFactory, DocumentDecoderFactory>();

        services.AddSingleton(_ => new DbContextFactory<DownloaderV3Context>().Create(ContextOption.Staging, "DownloaderV3"));
        services.AddSingleton<BaseDestination>(provider =>
        {
            var context = provider.GetRequiredService<DownloaderV3Context>();
            return context;
        });

        services.AddTransient(typeof(DownloadHandler<>));
    }

    public async Task<IEnumerable<ResultObject>> RunAsync(ILambdaContext lambdaContext)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<LambdaFunction>>();
        ApplicationLogger.Initialize(logger);

        var downloadHandler = _serviceProvider.GetRequiredService<DownloadHandler<InputData>>();

        return await downloadHandler.HandleAsync();
    }
}