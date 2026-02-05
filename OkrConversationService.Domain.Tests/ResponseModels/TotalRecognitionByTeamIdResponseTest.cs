using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class TotalRecognitionByTeamIdResponseTest : BaseTest
    {
        [Fact]
        public void TotalRecognitionByTeamIdResponse_Success()
        {
            var model = new TotalRecognitionByTeamIdResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

    }
}
