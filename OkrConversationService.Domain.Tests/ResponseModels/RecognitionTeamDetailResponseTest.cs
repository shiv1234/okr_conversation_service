using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class RecognitionTeamDetailResponseTest :BaseTest
    {
        [Fact]
        public void RecognitionTeamDetailResponse_Success()
        {
            var model = new RecognitionTeamDetailResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
