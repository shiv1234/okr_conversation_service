using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{

    public class RecognitionTeamsResponseTest : BaseTest
    {
        [Fact]
        public void RecognitionTeamsResponseTest_Success()
        {
            var model = new RecognitionTeamsResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
