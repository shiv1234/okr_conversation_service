using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class RecognitionEditCommandTest : BaseTest
    {
        [Fact]
        public void RecognitionEditCommandTest_Success()
        {
            var model = new RecognitionEditCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
