using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class EmployeeRolePermissionResponseTest : BaseTest
    {
        [Fact]
        public void GetDirectReportsDetailsByIdResponse_Success()
        {
            var model = new EmployeeRolePermissionResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}