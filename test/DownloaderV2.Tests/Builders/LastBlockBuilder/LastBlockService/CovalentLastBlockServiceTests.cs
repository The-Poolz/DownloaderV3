using DownloaderV2.Models.LastBlock;
using DownloaderV2.Builders.LastBlockBuilder.LastBlockService;
using Newtonsoft.Json;
using Flurl.Http.Testing;

namespace DownloaderV2.Tests.Builders.LastBlockBuilder.LastBlockService;

public class CovalentLastBlockServiceTests
{
    private readonly HttpTest _httpTest;

    public CovalentLastBlockServiceTests()
    {
        Environment.SetEnvironmentVariable("LastBlockDownloaderUrl", "https://test?");
        Environment.SetEnvironmentVariable("LastBlockKey", "key");

        _httpTest = new HttpTest();
    }

    [Fact]
    public async Task FetchLastBlockDataAsync_ReturnsCorrectData()
    {
        _httpTest.RespondWith("{\"data\":{\"items\":[{\"chain_id\":1,\"synced_block_height\":100}]}}");

        var service = new CovalentLastBlockService();

        var result = await service.FetchLastBlockDataAsync();

        Assert.NotNull(result);
        Assert.Equal(100, result[1]);
        Assert.Single(result);
        Assert.Equal("https://test?key", service.GetUri);

        _httpTest.ShouldHaveCalled("https://test?key");
    }

    [Fact]
    public void Deserialize_JsonIntoLastBlockResponse_PopulatesCorrectly()
    {
        const string json = @"
            {
            'data': {
                'updated_at': '2021-07-21T17:32:00Z',
                'items': [
                    {
                        'chain_id': 1,
                        'synced_block_height': 100
                    }
                ],
                'pagination': 'page=2'
                }
            }";

        var response = JsonConvert.DeserializeObject<LastBlockResponse>(json);

        Assert.NotNull(response);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data.Items!);
        Assert.Equal(1, response.Data.Items![0].ChainId);
        Assert.Equal(100, response.Data.Items[0].BlockHeight);
        Assert.Equal("page=2", response.Data.Pagination);
        Assert.Equal(new DateTime(2021, 7, 21, 17, 32, 0, DateTimeKind.Utc), response.Data.UpdatedAt);
    }
}