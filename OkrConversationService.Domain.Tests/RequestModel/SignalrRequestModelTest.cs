using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{

    public class SignalrRequestModelTest : BaseTest
    {
        [Fact]
        public void SignalrRequestModelRequest_Success()
        {
            var model = new SignalrRequestModel();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}

