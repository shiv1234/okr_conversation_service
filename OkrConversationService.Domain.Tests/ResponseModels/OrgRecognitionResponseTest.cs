using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class OrgRecognitionResponseTest : BaseTest
    {
        [Fact]
        public void OrgRecognitionResponse_Success()
        {
            var model = new OrgRecognitionResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void ReceiverDetail_Success()
        {
            var model = new ReceiverDetail();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        
    }
}
