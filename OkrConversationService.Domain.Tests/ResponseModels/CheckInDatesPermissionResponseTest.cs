using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class CheckInDatesPermissionResponseTest : BaseTest
    {
        [Fact]
        public void CheckInDatesPermissionResponse_Success()
        {
            var model = new CheckInDatesPermissionResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
