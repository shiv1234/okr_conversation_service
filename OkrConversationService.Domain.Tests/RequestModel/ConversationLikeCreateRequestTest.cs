using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class ConversationLikeCreateRequestTest : BaseTest
    {
        [Fact]
        public void ConversationLikeCreateRequest_Success()
        {
            var model = new Identity();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
