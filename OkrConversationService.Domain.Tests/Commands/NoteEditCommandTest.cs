using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class NoteEditCommandTest : BaseTest
    {
        [Fact]
        public void NoteEditCommand_Success()
        {
            var model = new NoteEditCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
