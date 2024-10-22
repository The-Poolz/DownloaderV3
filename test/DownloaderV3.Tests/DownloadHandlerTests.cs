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
using DownloaderV3.Helpers;

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

    private ServiceProvider CreateServiceProvider()
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

        return services.BuildServiceProvider();
    }

    [Fact]
    public void Constructor_ShouldResolveDependenciesCorrectly()
    {
        var serviceProvider = CreateServiceProvider();

        var handler = new DownloadHandler<InputData>(serviceProvider);

        handler.Should().NotBeNull("because the handler should be correctly resolved by the service provider");
    }

    [Fact]
    public void Constructor_ShouldResolveDependenciesCorrectlyWithDestination()
    {
        var serviceProvider = CreateServiceProvider();
        var destination = serviceProvider.GetRequiredService<CovalentContext>();

        var handler = new DownloadHandler<InputData>(destination);

        handler.Should().NotBeNull("because the handler should be correctly instantiated with provided destination");
    }

    [Fact]
    public async Task HandleAsync_ShouldProcessContractsAndSaveData()
    {
        using var httpTest = new HttpTest();

        httpTest
            .RespondWith(CovalentResultConst.LastBlockString)
            .RespondWith(CovalentResultConst.EventString);

        var serviceProvider = CreateServiceProvider();

        await using (var context = serviceProvider.GetRequiredService<CovalentContext>())
        {
            context.DownloaderSettings.Add(MockSettings.DownloaderSettings);
            context.DownloaderMapping.AddRange(MockSettings.DownloaderMappings);
            context.ChainsInfo.Add(MockSettings.ChainInfo);
            await context.SaveChangesAsync();
        }

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Application started.");

        var handler = new DownloadHandler<InputData>(serviceProvider);
        var results = await handler.HandleAsync();

        var resultObjects = results.ToList();
        resultObjects.Should().NotBeNull();

        resultObjects.First().ToString().Should().Contain("| 2 saved | ChainID     97 | SwapParty\n");
    }

    [Fact]
    public async Task HandleAsync_ShouldProcessContractsAndSaveDataWithServiceProviderByDefault()
    {
        using var httpTest = new HttpTest();

        httpTest
            .RespondWith(CovalentResultConst.LastBlockString)
            .RespondWith(CovalentResultConst.EventString);

        var options = new DbContextOptionsBuilder<CovalentContext>()
            .UseInMemoryDatabase("InMemoryDb")
            .Options;

        await using var context = new CovalentContext(options);
        
        context.DownloaderSettings.Add(MockSettings.DownloaderSettings);
        context.DownloaderMapping.AddRange(MockSettings.DownloaderMappings);
        context.ChainsInfo.Add(MockSettings.ChainInfo);
        await context.SaveChangesAsync();

        var handler = new DownloadHandler<InputData>(context);
        var results = await handler.HandleAsync();

        var resultObjects = results.ToList();
        resultObjects.Should().NotBeNull();

        resultObjects.First().ToString().Should().Contain("| 2 saved | ChainID     97 | SwapParty\n");
    }
}