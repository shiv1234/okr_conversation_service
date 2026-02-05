using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class RecognitionByTeamIdGetQueryTest : BaseTest
    {
        [Fact]
        public void RecognitionByTeamIdGetQuery_Success()
        {
            var model = new EmployeesLeaderBoardGetQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
