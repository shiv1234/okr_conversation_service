using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class ServiceSettingUrlResponseTest : BaseTest
    {
        [Fact]
        public void ServiceSettingUrlResponse_Success()
        {
            var model = new ServiceSettingUrlResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
