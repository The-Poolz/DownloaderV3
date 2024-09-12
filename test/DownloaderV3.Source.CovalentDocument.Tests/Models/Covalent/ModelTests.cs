using Newtonsoft.Json;
using FluentAssertions;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument.Tests.Models.Covalent
{
    public class ModelTests
    {
        [Fact]
        public void InputData_ShouldDeserializeCorrectly()
        {
            var result = JsonConvert.DeserializeObject<InputData>(CovalentResultConst.EventString);

            result.Should().NotBeNull();
            result!.Data.Should().NotBeNull();
            result.Data.ChainId.Should().Be(56);
            result.Data.Pagination.Should().NotBeNull();
            result.Data.Pagination.HasMore.Should().BeFalse();
            result.Data.Pagination.PageNumber.Should().Be(0);
            result.Error.Should().BeFalse();
        }

        [Fact]
        public void Clone_ShouldCreateADeepCopy()
        {
            var original = new Data
            {
                ChainId = 1,
                UpdatedAt = DateTime.UtcNow,
                Items = new Transaction[] { },
                Pagination = new Pagination { HasMore = true, PageNumber = 1 }
            };

            var cloned = original.Clone();

            cloned.Should().NotBeSameAs(original);
            cloned.ChainId.Should().Be(original.ChainId);
            cloned.UpdatedAt.Should().Be(original.UpdatedAt);
            cloned.Items.Should().BeEquivalentTo(original.Items);
            cloned.Pagination.Should().BeEquivalentTo(original.Pagination);
        }
    }
}
