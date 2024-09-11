using FluentAssertions;
using Flurl.Http.Testing;
using SourceLastBlock.Helpers;

namespace SourceLastBlock.Tests
{
    public class GetLastBlockTests : IDisposable
    {
        private readonly HttpTest _httpTest;
        public GetLastBlockTests()
        {
            _httpTest = new HttpTest();

            Environment.SetEnvironmentVariable("LastBlockDownloaderUrl", "https://api?");
            Environment.SetEnvironmentVariable("LastBlockKey", "key");
        }

        public void Dispose() => _httpTest.Dispose();

        [Fact]
        public  void GetResponse_ShouldReturnValidJToken()
        {
            var getLastBlock = new GetLastBlockCovalent();
            var fakeJsonResponse = "{\"data\": { \"items\": [{\"ChainId\": 1, \"BlockHeight\": 100}, {\"ChainId\": 2, \"BlockHeight\": 200}] }}";

            _httpTest.RespondWith(fakeJsonResponse);

            var result = getLastBlock.GetResponse();

            result.Should().NotBeNull();
            result.Should().BeOfType<string>();
            result!.Should().Be(fakeJsonResponse);
        }

        [Fact]
        public void ParseResponse_ValidJson_ShouldReturnCorrectDictionary()
        {
            var jsonData = "{\"data\":{\"items\":[{\"chain_id\":1,\"synced_block_height\":100}]}}";
            var expectedDictionary = new Dictionary<long, long>
            {
                { 1, 100 }
            };

            var getLastBlock = new GetLastBlockCovalent();

            var result = getLastBlock.ParseResponse(jsonData);

            result.Should().BeEquivalentTo(expectedDictionary);
        }

        [Fact]
        public void ParseResponse_InvalidJson_ThrowsException()
        {
            var jsonData = "{\"unexpected_field\": 1}";
            var service = new GetLastBlockCovalent();

            Action act = () => service.ParseResponse(jsonData);

            Assert.Throws<InvalidOperationException>(() => service.ParseResponse(jsonData));
            act.Should().Throw<InvalidOperationException>().WithMessage(ExceptionMessages.FailedToRetrieveLastBlockData);
        }

        [Fact]
        public void ParseResponse_NullJson_ShouldThrowException()
        {
            var getLastBlock = new GetLastBlockCovalent();

            Action act = () => getLastBlock.ParseResponse(null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'value')");
        }

        [Fact]
        public void FetchData_ShouldReturnParsedDictionary()
        {
            var getLastBlock = new GetLastBlockCovalent();
            var fakeJsonResponse = "{\"data\": { \"items\": [{\"chain_id\": 1, \"synced_block_height\": 100}, {\"chain_id\": 2, \"synced_block_height\": 200}] }}";

            var expectedDictionary = new Dictionary<long, long>
            {
                { 1, 100 },
                { 2, 200 }
            };

            _httpTest.RespondWith(fakeJsonResponse);

            var result = getLastBlock.FetchData();

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDictionary);
        }
    }
}