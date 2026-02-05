using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class CheckInGetAllQueryTest : BaseTest
    {
        [Fact]
        public void CheckInGetAllQueryTest_Success()
        {
            var model = new CheckInGetAllQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}