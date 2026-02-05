using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class ConversationCreateCommandTest : BaseTest
    {
        [Fact]
        public void ConversationCreateCommand_Success()
        {
            var model = new ConversationCreateCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
