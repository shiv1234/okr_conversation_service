using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class ConversationEmployeeTagResponseTest : BaseTest
    {
        [Fact]
        public void ConversationEmployeeTagResponse_Success()
        {
            var model = new ConversationEmployeeTagResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
