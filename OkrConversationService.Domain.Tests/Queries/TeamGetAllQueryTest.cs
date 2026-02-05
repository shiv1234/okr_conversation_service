using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class TeamGetAllQueryTest : BaseTest
    {
        [Fact]
        public void TeamGetAllQuery_Success()
        {
            var model = new TeamGetAllQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
