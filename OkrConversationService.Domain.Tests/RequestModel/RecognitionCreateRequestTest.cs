using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class RecognitionCreateRequestTest : BaseTest
    {
        [Fact]
        public void RecognitionCreateRequest_Success()
        {
            var model = new RecognitionCreateRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
