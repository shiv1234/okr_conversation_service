using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class RecognitionCreateCommandTest : BaseTest
    {
        [Fact]
        public void RecognitionCreateCommand_Success()
        {
            var model = new RecognitionCreateCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
