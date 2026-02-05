using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class RecognitionGroupFilterWallofFameRequest : BaseTest
    {
        [Fact]
        public void RecognitionGroupFilterWallofFameRequest_Success()
        {
            var model = new Domain.RequestModel.RecognitionGroupFilterWallofFame();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
