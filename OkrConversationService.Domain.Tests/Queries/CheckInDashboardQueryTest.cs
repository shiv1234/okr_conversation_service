using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class CheckInDashboardQueryTest : BaseTest
    {
        [Fact]
        public void CheckInDashboardQueryTest_Success()
        {
            var model = new CheckInDashboardQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
