using DownloaderV3.Destination;
using Microsoft.Extensions.DependencyInjection;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentLastBlock.SourcePage;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;
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
        services.AddTransient<DownloadHandler<InputData>>();

        return services;
    }
}