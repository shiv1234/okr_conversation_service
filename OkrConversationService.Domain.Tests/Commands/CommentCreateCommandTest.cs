using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class CommentCreateCommandTest : BaseTest
    {
        [Fact]
        public void CommentCreateCommand_Success()
        {
            var model = new CommentCreateCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
