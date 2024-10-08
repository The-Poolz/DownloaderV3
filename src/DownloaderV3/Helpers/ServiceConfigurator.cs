using DownloaderV3.Destination;
using Microsoft.Extensions.Logging;
using DownloaderV3.Source.CovalentLastBlock;
using Microsoft.Extensions.DependencyInjection;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentLastBlock.SourcePage;
using DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;

namespace DownloaderV3.Helpers;

public static class ServiceConfigurator
{
    public static void ConfigureServices<TContext>(IServiceCollection services, Func<IServiceProvider, TContext> dbContextFactory)
        where TContext : BaseDestination
    {
        services.AddLogging(config =>
        {
            config.AddConsole();
        });

        if (services.All(s => s.ServiceType != typeof(GetSourcePage)))
        {
            services.AddTransient<GetSourcePage, GetLastBlockCovalent>();
        }

        if (services.All(s => s.ServiceType != typeof(IDocumentFactory)))
        {
            services.AddTransient<IDocumentFactory, DocumentFactory>();
        }

        if (services.All(s => s.ServiceType != typeof(IDocumentDecoderFactory)))
        {
            services.AddTransient<IDocumentDecoderFactory, DocumentDecoderFactory>();
        }

        services.AddSingleton(dbContextFactory);

        services.AddSingleton<BaseDestination>(provider =>
        {
            var context = provider.GetRequiredService<TContext>();
            return context;
        });

        services.AddTransient(typeof(DownloadHandler<>));
    }
}