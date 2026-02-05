using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{

    public class RecognitionTeamRequestTest : BaseTest
    {
        [Fact]
        public void RecognitionTeamRequestTest_Success()
        {
            var model = new RecognitionTeamRequestTest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
