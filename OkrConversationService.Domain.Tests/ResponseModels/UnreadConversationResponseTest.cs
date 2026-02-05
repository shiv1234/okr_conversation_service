using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class UnreadConversationResponseTest : BaseTest
    {
        [Fact]
        public void UnreadConversationResponse_Success()
        {
            var model = new UnreadConversationResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
