using DownloaderV2.Builders.LastBlockBuilder.SourcePage;
using DownloaderV2.Models.LastBlock;
using Flurl.Http.Testing;
using Newtonsoft.Json.Linq;

namespace DownloaderV2.Tests.Builders.LastBlockBuilder.SourcePage;

public class GetLastBlockTests
{
    private readonly HttpTest _httpTest;

    public GetLastBlockTests()
    {
        Environment.SetEnvironmentVariable("LastBlockDownloaderUrl", "https://api?");
        Environment.SetEnvironmentVariable("LastBlockKey", "key");

        _httpTest = new HttpTest();
    }

    [Fact]
    public void Constructor_SetsCorrectUri()
    {
        const string expectedUri = "https://api?key";

        var service = new GetLastBlock();

        Assert.Equal(expectedUri, service.GetUri);
    }

    [Fact]
    public void DeserializeResponse_ValidJson_ReturnsExpectedData()
    {
        var jsonData = JToken.Parse("{\"data\":{\"items\":[{\"chain_id\":1,\"synced_block_height\":100}]}}");
        var service = new GetLastBlock();

        var result = service.DeserializeResponse(jsonData);

        Assert.NotNull(result);
        Assert.Equal(100, result.Data.Items!.FirstOrDefault()!.BlockHeight);
    }

    [Fact]
    public void DeserializeResponse_InvalidJson_DataNull()
    {
        var jsonData = JToken.Parse("{\"unexpected_field\": 1}"); 
        var service = new GetLastBlock();

        var serialize =  service.DeserializeResponse(jsonData);
        Assert.Null(serialize.Data);
    }

    [Fact]
    public void PopulateDataDictionary_ValidData_ReturnsCorrectDictionary()
    {
        var items = new[]
        {
            new Item { ChainId = 1, BlockHeight = 100 },
            new Item { ChainId = 2, BlockHeight = 200 }
        };

        var lastBlockData = new LastBlockResponse
        {
            Data = new Data
            {
                UpdatedAt = DateTime.UtcNow,
                Items = items,
                Pagination = "None"
            }
        };

        var service = new GetLastBlock();

        var dictionary = service.PopulateDataDictionary(lastBlockData);

        Assert.NotNull(dictionary);
        Assert.Equal(100, dictionary[1]);
        Assert.Equal(200, dictionary[2]);
        Assert.Equal(2, dictionary.Count);
    }

    [Fact]
    public async Task GetResponseAsync_CallsExpectedUrl_ReturnsData()
    {
        var expectedResponse = JToken.Parse("{\"data\":{\"items\":[{\"chain_id\":1,\"synced_block_height\":100}]}}");
        var expectedUri = "https://api?key";

        _httpTest.RespondWithJson(expectedResponse);
        var service = new GetLastBlock();

        var result = await service.GetResponseAsync(expectedUri);

        Assert.Equal(expectedResponse, result);
    }
}