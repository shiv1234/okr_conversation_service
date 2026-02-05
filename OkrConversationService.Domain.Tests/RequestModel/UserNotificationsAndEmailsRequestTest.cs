using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class UserNotificationsAndEmailsRequestTest : BaseTest
    {
        [Fact]
        public void UserNotificationsAndEmailsRequest_Success()
        {
            var model = new UserNotificationsAndEmailsRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
