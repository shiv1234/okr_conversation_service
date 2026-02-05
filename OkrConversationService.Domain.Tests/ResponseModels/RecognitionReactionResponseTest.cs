using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class RecognitionReactionResponseTest : BaseTest
    {
        [Fact]
        public void RecognitionReactionResponse_Success()
        {
            var model = new RecognitionReactionResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
