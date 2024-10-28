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
    public static IServiceProvider BuildServiceProvider(BaseDestination destination, Action<ILoggingBuilder>? loggingConfiguration)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton(destination);

        serviceCollection.AddLogging(config =>
        {
            if (loggingConfiguration != null)
                loggingConfiguration(config);
            else
                config.AddConsole();
        });

        if (serviceCollection.All(s => s.ServiceType != typeof(GetSourcePage))) serviceCollection.AddTransient<GetSourcePage, GetLastBlockCovalent>();
        if (serviceCollection.All(s => s.ServiceType != typeof(IDocumentFactory))) serviceCollection.AddTransient<IDocumentFactory, DocumentFactory>();
        if (serviceCollection.All(s => s.ServiceType != typeof(IDocumentDecoderFactory))) serviceCollection.AddTransient<IDocumentDecoderFactory, DocumentDecoderFactory>();

        return serviceCollection.BuildServiceProvider();
    }
}