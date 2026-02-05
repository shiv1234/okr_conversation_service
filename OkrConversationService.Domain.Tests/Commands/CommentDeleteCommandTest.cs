using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class CommentDeleteCommandTest : BaseTest
    {
        [Fact]
        public void CommentDeleteCommand_Success()
        {
            var model = new CommentDeleteCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
