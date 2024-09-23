using DownloaderV3.Helpers;
using DownloaderV3.Destination;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentLastBlock.SourcePage;
using DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;

namespace DownloaderV3.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDownloaderServices(this IServiceCollection services)
    {
        services.AddTransient<GetSourcePage>();
        services.AddTransient<BaseDestination>();
        services.AddTransient<IDocumentFactory>();
        services.AddTransient<IDocumentDecoderFactory>();
        services.AddTransient(typeof(DownloadHandler<>));

        services.AddLogging(config => config.AddConsole());

        var serviceProvider = services.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<ApplicationLogger>>();
        ApplicationLogger.Initialize(logger);

        return services;
    }
}