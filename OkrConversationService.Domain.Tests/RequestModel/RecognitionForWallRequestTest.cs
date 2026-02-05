using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class RecognitionForWallRequestTest : BaseTest
    {
        [Fact]
        public void RecognitionForWallRequest_Success()
        {
            var model = new Domain.RequestModel.RecognitionForWallRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
