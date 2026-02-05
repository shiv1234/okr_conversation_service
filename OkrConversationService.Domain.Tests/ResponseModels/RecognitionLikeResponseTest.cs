using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class RecognitionLikeResponseTest : BaseTest
    {
        [Fact]
        public void RecognitionLikeResponse_Success()
        {
            var model = new RecognitionLikeResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
