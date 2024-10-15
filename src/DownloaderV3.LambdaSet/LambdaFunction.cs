using Amazon.Lambda.Core;
using DownloaderV3.Result;
using DownloaderV3.Helpers;
using DownloaderV3.DataBase;
using Microsoft.Extensions.Logging;
using ConfiguredSqlConnection.Extensions;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace DownloaderV3.LambdaSet;

public class LambdaFunction(DownloaderV3Context context)
{
    public readonly DownloaderV3Context Context = context;

    public LambdaFunction()
        : this(new DbContextFactory<DownloaderV3Context>().Create(ContextOption.Staging, "DownloaderV3"))
    { }

    public async Task<IEnumerable<ResultObject>> RunAsync(ILambdaContext lambdaContext)
    {
        ApplicationLogger.Initialize(LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<LambdaFunction>());

        return await new DownloadHandler<InputData>(Context).HandleAsync();
    }
}