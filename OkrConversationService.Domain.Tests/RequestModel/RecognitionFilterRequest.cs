using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class RecognitionFilterRequest : BaseTest
    {
        [Fact]
        public void RecognitionFilterRequest_Success()
        {
            var model = new Domain.RequestModel.RecognitionFilter();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
