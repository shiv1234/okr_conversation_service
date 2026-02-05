using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class RecognitionByTeamIdResponseTest : BaseTest
    {
        [Fact]
        public void RecognitionByTeamIdResponse_Success()
        {
            var model = new RecognitionByTeamIdResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
