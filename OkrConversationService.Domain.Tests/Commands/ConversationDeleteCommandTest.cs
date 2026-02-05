using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class ConversationDeleteCommandTest : BaseTest
    {
        [Fact]
        public void ConversationDeleteCompleted_Success()
        {
            var model = new ConversationDeleteCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
