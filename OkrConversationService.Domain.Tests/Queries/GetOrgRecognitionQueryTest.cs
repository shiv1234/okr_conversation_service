using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class GetOrgRecognitionQueryTest : BaseTest
    {
        [Fact]
        public void GetOrgRecognitionQuery_Success()
        {
            var model = new GetOrgRecognitionQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
