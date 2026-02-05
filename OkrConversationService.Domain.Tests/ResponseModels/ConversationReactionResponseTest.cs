using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class ConversationReactionResponseTest : BaseTest
    {
        [Fact]
        public void ConversationReactionResponse_Success()
        {
            var model = new ConversationReactionResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
