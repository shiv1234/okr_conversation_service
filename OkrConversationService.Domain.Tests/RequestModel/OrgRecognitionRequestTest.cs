using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public  class OrgRecognitionRequestTest : BaseTest
    {
        [Fact]
        public void OrgRecognitionRequestTest_Success()
        {
            var model = new OrgRecognitionRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
