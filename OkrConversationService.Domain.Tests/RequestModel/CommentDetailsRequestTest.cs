using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class CommentDetailsRequestTest : BaseTest
    {
        [Fact]
        public void CommentDetailsRequest_Success()
        {
            var model = new CommentDetailsRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
