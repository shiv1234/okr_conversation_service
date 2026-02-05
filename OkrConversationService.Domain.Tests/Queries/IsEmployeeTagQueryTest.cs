
using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class IsEmployeeTagQueryTest : BaseTest
    {
        [Fact]
        public void IsEmployeeTagQuery_Success()
        {
            var model = new IsEmployeeTagQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
