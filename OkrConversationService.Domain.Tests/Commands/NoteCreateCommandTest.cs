using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class NoteCreateCommandTest : BaseTest
    {
        [Fact]
        public void NoteCreateCommand_Success()
        {
            var model = new NoteCreateCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
