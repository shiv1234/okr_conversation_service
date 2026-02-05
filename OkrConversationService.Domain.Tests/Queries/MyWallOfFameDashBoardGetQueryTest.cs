using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class MyWallOfFameDashBoardGetQueryTest : BaseTest
    {
        [Fact]
        public void MyWallOfFameDashBoardGetQuery_Success()
        {
            var model = new MyWallOfFameDashBoardGetQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
