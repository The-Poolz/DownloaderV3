using DownloaderV3.DataBase;
using Microsoft.Extensions.Logging;
using ConfiguredSqlConnection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;
using Amazon.Lambda.Core;
using DownloaderV3.Helpers;
using DownloaderV3.Result;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace DownloaderV3.LambdaSet;

public class LambdaFunction
{
    private readonly ServiceProvider _serviceProvider;

    public LambdaFunction()
    {
        var services = new ServiceCollection();

        ServiceConfigurator.ConfigureServices<DownloaderV3Context>(
            services,
            _ => new DbContextFactory<DownloaderV3Context>().Create(ContextOption.Prod)
        );

        _serviceProvider = services.BuildServiceProvider();
    }

    public async Task<IEnumerable<ResultObject>> RunAsync(ILambdaContext lambdaContext)
    {
        ApplicationLogger.Initialize(_serviceProvider.GetRequiredService<ILogger<LambdaFunction>>());

        var downloadHandler = _serviceProvider.GetRequiredService<DownloadHandler<InputData>>();

        return await downloadHandler.HandleAsync();
    }
}