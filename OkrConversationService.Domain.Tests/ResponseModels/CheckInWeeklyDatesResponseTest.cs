using OkrConversationService.Domain.ResponseModels;
using Xunit;


namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class CheckInWeeklyDatesResponseTest : BaseTest
    {
        [Fact]
        public void GetAllResponse_Success()
        {
            var model = new CheckInWeeklyDatesResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}