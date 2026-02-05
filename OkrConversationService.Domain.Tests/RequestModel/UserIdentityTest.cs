using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{

    public class UserIdentityTest : BaseTest
    {
        [Fact]
        public void UserIdentityRequest_Success()
        {
            var model = new UserIdentity();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}

