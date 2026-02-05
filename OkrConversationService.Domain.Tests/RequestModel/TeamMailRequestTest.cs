using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class TeamMailRequestTest : BaseTest
    {
        [Fact]
        public void TeamMailRequest_Success()
        {
            var model = new TeamMailRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
