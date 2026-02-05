using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class CheckInWeeklyDatesQueryTest : BaseTest
    {
        [Fact]
        public void CheckInWeeklyDatesQueryTest_Success()
        {
            var model = new CheckInWeeklyDatesQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}