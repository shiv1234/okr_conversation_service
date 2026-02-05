using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class RecognitionLikeQueryTest : BaseTest
    {
        [Fact]
        public void ConversationGetAllQuery_Success()
        {
            var model = new RecognitionLikeQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
