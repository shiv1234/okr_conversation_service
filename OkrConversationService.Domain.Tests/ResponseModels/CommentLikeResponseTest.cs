using OkrConversationService.Domain.ResponseModels;
using Xunit;


namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class CommentLikeResponseTest :BaseTest
    {
        [Fact]
        public void CommentLikeResponse_Success()
        {
            var model = new CommentLikeResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
