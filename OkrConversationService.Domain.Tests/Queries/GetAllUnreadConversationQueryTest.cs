
using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class GetAllUnreadConversationQueryTest : BaseTest
    {
        [Fact]
        public void GetAllUnreadConversationQuery_Success()
        {
            var model = new GetAllUnreadConversationQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
