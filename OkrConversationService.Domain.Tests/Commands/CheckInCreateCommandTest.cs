using OkrConversationService.Domain.Commands;
using Xunit;

namespace OkrConversationService.Domain.Tests.Commands
{
    public class CheckInCreateCommandTest : BaseTest
    {
        [Fact]
        public void CheckInCreateCommand_Success()
        {
            var model = new CheckInCreateCommand();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}