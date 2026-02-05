using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class RecognitionDetailsResponseTest : BaseTest
    {
        [Fact]
        public void RecognitionDetailsResponse_Success()
        {
            var model = new Domain.ResponseModels.RecognitionDetailsResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
