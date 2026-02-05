using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class DashboardCheckInResponseTest : BaseTest
    {
        [Fact]
        public void GetAllResponse_Success()
        {
            var model = new DashboardCheckInResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
