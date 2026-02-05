using OkrConversationService.Domain.RequestModel;
using Xunit;
namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class CreateEngagementReportRequestTest : BaseTest
    {

        [Fact]
        public void CreateEngagementReportRequest_Success()
        {
            var model = new CreateEngagementReportRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
