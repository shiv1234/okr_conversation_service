using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class CreateRecognitionRequestTest : BaseTest
    {
        [Fact]
        public void CreateRecognitionRequest_Success()
        {
            var model = new RecognitionCreateRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
