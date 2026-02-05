using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class RecognitionEditRequestTest : BaseTest
    {
        [Fact]
        public void RecognitionEditRequest_Success()
        {
            var model = new RecognitionEditRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
