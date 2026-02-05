using OkrConversationService.Domain.RequestModel;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class EmployeesLeaderBoardRequest : BaseTest
    {
        [Fact]
        public void EmployeesLeaderBoardRequest_Success()
        {
            var model = new Domain.RequestModel.EmployeesLeaderBoardRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
