using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class IsCheckInSubmittedQueryTest : BaseTest
    {
        [Fact]
        public void IsCheckInSubmittedQueryTest_Success()
        {
            var model = new IsCheckInSubmittedQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}