using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class BlockedWordsRequestTest : BaseTest
    {
        [Fact]
        public void BlockedWordsRequest_Success()
        {
            var model = new BlockedWordsRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
