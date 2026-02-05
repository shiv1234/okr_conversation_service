using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class RecognitionDeleteCommandTest : BaseTest
    {
        [Fact]
        public void RecognitionDeleteCommand_Success()
        {
            var model = new RecognitionDeleteCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
