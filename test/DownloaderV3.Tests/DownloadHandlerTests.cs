using FluentAssertions;
using Flurl.Http.Testing;
using DownloaderV3.Tests.Mock;
using DownloaderV3.Destination;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DownloaderV3.Tests.Mock.Db.Models;
using DownloaderV3.Source.CovalentLastBlock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentLastBlock.SourcePage;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;
using DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;

namespace DownloaderV3.Tests;

public class DownloadHandlerTests
{
    public DownloadHandlerTests()
    {
        Environment.SetEnvironmentVariable("LastBlockDownloaderUrl", "https://api?");
        Environment.SetEnvironmentVariable("LastBlockKey", "key");
        Environment.SetEnvironmentVariable("AWS_REGION", "us-east-1");
        Environment.SetEnvironmentVariable("ApiUrl", "test");
    }

    [Fact]
    public void Constructor_ShouldResolveDependenciesCorrectly()
    {
        var services = new ServiceCollection();

        services.AddLogging(config => config.AddConsole());
        services.AddDbContext<CovalentContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryCovalentDb");
        });

        services.AddScoped<BaseDestination, CovalentContext>();
        services.AddTransient<GetSourcePage, GetLastBlockCovalent>();
        services.AddTransient<IDocumentFactory, DocumentFactory>();
        services.AddTransient<IDocumentDecoderFactory, DocumentDecoderFactory>();
        services.AddTransient(typeof(DownloadHandler<>));

        var serviceProvider = services.BuildServiceProvider();
        var handler = serviceProvider.GetRequiredService<DownloadHandler<InputData>>();

        handler.Should().NotBeNull();
    }

    [Fact]
    public async Task HandleAsync_ShouldProcessContractsAndSaveData()
    {
        using var httpTest = new HttpTest();

        httpTest
            .RespondWith(CovalentResultConst.LastBlockString)
            .RespondWith(CovalentResultConst.EventString);

        var services = new ServiceCollection();

        services.AddLogging(config =>
        {
            config.AddConsole();
        });

        services.AddDbContext<CovalentContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryCovalentDb");
        });

        services.AddScoped<BaseDestination, CovalentContext>();

        services.AddTransient<GetSourcePage, GetLastBlockCovalent>();
        services.AddTransient<IDocumentFactory, DocumentFactory>();
        services.AddTransient<IDocumentDecoderFactory, DocumentDecoderFactory>();

        services.AddTransient(typeof(DownloadHandler<>));

        var serviceProvider = services.BuildServiceProvider();

        await using (var context = serviceProvider.GetRequiredService<CovalentContext>())
        {
            context.DownloaderSettings.Add(MockSettings.DownloaderSettings);
            context.DownloaderMapping.AddRange(MockSettings.DownloaderMappings);
            context.ChainsInfo.Add(MockSettings.ChainInfo);
            await context.SaveChangesAsync();
        }

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Application started.");

        var downloadHandler = serviceProvider.GetRequiredService<DownloadHandler<InputData>>();

        var results = await downloadHandler.HandleAsync();

        var resultObjects = results.ToList();
        resultObjects.Should().NotBeNull();

        resultObjects.First().ToString().Should().Contain("| 2 saved | ChainID     97 | SwapParty\n");
    }
}