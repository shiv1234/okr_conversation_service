using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class ConversationLikeCommandTest : BaseTest
    {
        [Fact]
        public void ConversationLikeCommand_Success()
        {
            var model = new ConversationLikeCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
