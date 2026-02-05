using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class ConversationEditCommandTest : BaseTest
    {
        [Fact]
        public void ConversationEditCommand_Success()
        {
            var model = new ConversationEditCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
