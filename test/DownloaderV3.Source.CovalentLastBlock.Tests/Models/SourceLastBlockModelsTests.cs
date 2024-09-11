using Newtonsoft.Json;
using FluentAssertions;
using DownloaderV3.Source.CovalentLastBlock.Models.LastBlock;

namespace DownloaderV3.Source.CovalentLastBlock.Tests.Models;

public class SourceLastBlockModelsTests
{
    [Fact]
    public void DataModel_ShouldDeserializeCorrectly()
    {
        var jsonData = @"{ 'updated_at': '2024-01-01T00:00:00Z', 'items': [{ 'name': 'Test Chain', 'chain_id': 1, 'is_testnet': false, 'logo_url': 'http://example.com/logo.png', 'synced_block_height': 1000, 'synced_blocked_signed_at': '2024-01-01T00:00:00Z', 'has_data': true }], 'pagination': 'page_1' }";

        var result = JsonConvert.DeserializeObject<Data>(jsonData);

        result.Should().NotBeNull();
        result!.UpdatedAt.Should().Be(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        result.Items.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Pagination.Should().Be("page_1");
    }

    [Fact]
    public void ItemModel_ShouldDeserializeCorrectly()
    {
        var jsonItem = @"{ 'name': 'Test Chain', 'chain_id': 1, 'is_testnet': false, 'logo_url': 'http://example.com/logo.png', 'synced_block_height': 1000, 'synced_blocked_signed_at': '2024-01-01T00:00:00Z', 'has_data': true }";

        var result = JsonConvert.DeserializeObject<Item>(jsonItem);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Chain");
        result.ChainId.Should().Be(1);
        result.IsTestnet.Should().BeFalse();
        result.LogoUrl.Should().Be("http://example.com/logo.png");
        result.BlockHeight.Should().Be(1000);
        result.SignedAt.Should().Be(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        result.HasData.Should().BeTrue();
    }

    [Fact]
    public void LastBlockResponseModel_ShouldDeserializeCorrectly()
    {
        var jsonLastBlockResponse = @"{ 'data': { 'updated_at': '2024-01-01T00:00:00Z', 'items': [{ 'name': 'Test Chain', 'chain_id': 1, 'is_testnet': false, 'logo_url': 'http://example.com/logo.png', 'synced_block_height': 1000, 'synced_blocked_signed_at': '2024-01-01T00:00:00Z', 'has_data': true }], 'pagination': 'page_1' } }";

        var result = JsonConvert.DeserializeObject<LastBlockResponse>(jsonLastBlockResponse);

        result.Should().NotBeNull();
        result!.Data.Should().NotBeNull();
        result.Data.Items.Should().NotBeNull();
        result.Data.Items.Should().HaveCount(1);
        result.Data.Pagination.Should().Be("page_1");
    }
}