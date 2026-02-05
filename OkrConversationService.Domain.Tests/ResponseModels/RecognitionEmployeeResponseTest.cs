using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{

    public class RecognitionEmployeeResponseTest : BaseTest
    {
        [Fact]
        public void RecognitionEmployeeResponse_Success()
        {
            var model = new RecognitionEmployeeResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
