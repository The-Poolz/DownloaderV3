using Newtonsoft.Json;
using FluentAssertions;
using DownloaderV3.Source.CovalentDocument.Models.Covalent;

namespace DownloaderV3.Source.CovalentDocument.Tests.Models.Covalent
{
    public class ModelTests
    {
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

            result.Data.Should().NotBeNull();
            result.Data.ChainId.Should().Be(56);
            result.Data.Items.Should().HaveCount(2);

            var firstItem = result.Data.Items.First();
            firstItem.TxHash.Should().Be("0x60e192550c1ce8c7f88f98d0383d46cc57324724bdd551334f84ee0fab1ebxx1");
            firstItem.Decoded.Should().NotBeNull();
            firstItem.Decoded!.Name.Should().Be("TransferIn");
            firstItem.Decoded.Params.Should().HaveCount(3);

            var param = firstItem.Decoded.Params.First();
            param.Name.Should().Be("Amount");
            param.Type.Should().Be("uint256");
            param.Decoded.Should().BeTrue();
            param.Value.Should().Be("503106497524363400000");

            result.Data.Pagination.Should().NotBeNull();
            result.Data.Pagination.HasMore.Should().BeFalse();
            result.Data.Pagination.PageNumber.Should().Be(0);
            result.Data.Pagination.PageSize.Should().Be(100);
        }
    }
}
