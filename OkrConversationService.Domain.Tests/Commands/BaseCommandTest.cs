using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{

    public class BaseCommandTest : BaseTest
    {
        [Fact]
        public void CheckInCreateCommand_Success()
        {
            var model = new BaseCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
