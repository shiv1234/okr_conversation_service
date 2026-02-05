using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class GetCommentQueryTest : BaseTest
    {
        [Fact]
        public void ConversationGetAllQuery_Success()
        {
            var model = new GetCommentQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
    
}
