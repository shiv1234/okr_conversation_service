using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class TotalRecognitionByTeamIdRequestTest : BaseTest
    {
        [Fact]
        public void TotalRecognitionByTeamIdRequest_Success()
        {
            var model = new TotalRecognitionByTeamIdRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
