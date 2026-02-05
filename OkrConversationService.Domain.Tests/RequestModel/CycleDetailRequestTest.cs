using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class CycleDetailRequestTest : BaseTest
    {
        [Fact]
        public void CycleDetailRequest_Success()
        {
            var model = new Identity();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
