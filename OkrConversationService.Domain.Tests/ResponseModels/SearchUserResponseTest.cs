using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class SearchUserResponseTest : BaseTest
    {
        [Fact]
        public void SearchUserResponse_Success()
        {
            var model = new SearchUserResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
