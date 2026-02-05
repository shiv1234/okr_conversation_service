using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class MyWallOfFameGetQueryTest : BaseTest
    {
        [Fact]
        public void MyWallOfFameGetQuery_Success()
        {
            var model = new MyWallOfFameGetQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
