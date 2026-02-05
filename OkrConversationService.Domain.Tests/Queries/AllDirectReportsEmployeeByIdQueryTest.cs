using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class AllDirectReportsEmployeeByIdQueryTest : BaseTest
    {
        [Fact]
        public void AllDirectReportsEmployeeByIdQueryTest_Success()
        {
            var model = new AllDirectReportsEmployeeByIdQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}