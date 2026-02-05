using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class EmployeePermissionResponseTest : BaseTest
    {
        [Fact]
        public void GetDirectReportsDetailsByIdResponse_Success()
        {
            var model = new EmployeePermissionResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}