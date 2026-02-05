using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class MyWallOfFameRequestTest : BaseTest
    {
        [Fact]
        public void MyWallOfFameRequestTest_Success()
        {
            var model = new MyWallOfFameRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
