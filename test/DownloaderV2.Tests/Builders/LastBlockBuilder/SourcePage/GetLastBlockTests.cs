using Flurl.Http.Testing;
using Newtonsoft.Json.Linq;
using DownloaderV2.Builders.LastBlockBuilder.SourcePage;

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
    public void ParseResponse_ValidJson_ReturnsExpectedData()
    {
        var jsonData = JToken.Parse("{\"data\":{\"items\":[{\"chain_id\":1,\"synced_block_height\":100}]}}");
        var service = new GetLastBlock();

        var result = service.ParseResponse(jsonData);

        Assert.NotNull(result);
        Assert.Equal(100, result.FirstOrDefault().Value);
        Assert.Equal(1, result.FirstOrDefault().Key);
    }

    [Fact]
    public void ParseResponse_InvalidJson_ThrowsException()
    {
        var jsonData = JToken.Parse("{\"unexpected_field\": 1}");
        var service = new GetLastBlock();

        Assert.Throws<NullReferenceException>(() => service.ParseResponse(jsonData));
    }

    [Fact]
    public async Task GetResponseAsync_CallsExpectedUrl_ReturnsData()
    {
        var expectedResponse = JToken.Parse("{\"data\":{\"items\":[{\"chain_id\":1,\"synced_block_height\":100}]}}");
        const string expectedUri = "https://api?key";

        _httpTest.RespondWithJson(expectedResponse);
        var service = new GetLastBlock();

        var result = await service.GetResponseAsync(expectedUri);

        Assert.Equal(expectedResponse, result);
    }
}