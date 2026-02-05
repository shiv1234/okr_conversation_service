using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class ImportPastTaskCommandTest : BaseTest
    {
        [Fact]
        public void ImportPastTaskCommand_Success()
        {
            var model = new ImportPastTaskCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
