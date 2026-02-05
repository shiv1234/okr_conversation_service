using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class TeamsByEmpIdGetQueryTest : BaseTest
    {
        [Fact]
        public void TeamsByEmpIdGetQuery_Success()
        {
            var model = new TeamsByEmpIdGetQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
