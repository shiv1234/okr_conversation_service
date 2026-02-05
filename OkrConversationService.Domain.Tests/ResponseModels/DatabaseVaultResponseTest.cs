using OkrConversationService.Domain.ResponseModels;
using Xunit;
namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class DatabaseVaultResponseTest : BaseTest
    {
        [Fact]
        public void GetAllResponse_Success()
        {
            var model = new DatabaseVaultResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}