using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class ConversationFileResponseTest : BaseTest
    {
        [Fact]
        public void ConversationFileResponse_Success()
        {
            var model = new ConversationFileResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

    }
}
