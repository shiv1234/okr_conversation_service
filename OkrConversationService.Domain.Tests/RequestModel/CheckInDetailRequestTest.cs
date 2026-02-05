using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class CheckInDetailRequestTest : BaseTest
    {
        [Fact]
        public void CheckInCreateRequest_Success()
        {
            var model = new CheckInDetailRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}