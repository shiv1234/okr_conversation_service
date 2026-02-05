using Moq;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.QueryHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.Adapters.QueryHandlers
{
    public class CheckInDashboardQueryHandlerTest
    {
        [Fact]
        public async Task CheckInDashboardQueryHandler_IsSuccessFalse()
        {
            //Arrange   

            var mockService = new Mock<ICheckInService>();
            var handler = new CheckInDashboardQueryHandler(mockService.Object);
            var command = new CheckInDashboardQuery() { EmployeeId = 1 };

            var payload = new Payload<DashboardCheckInResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetDashboardCheckInDetails(command)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CheckInDashboardQueryHandler_IsSuccessTrue()
        {    //Arrange               

            var mockService = new Mock<ICheckInService>();
            var handler = new CheckInDashboardQueryHandler(mockService.Object);
            var query = new CheckInDashboardQuery() { EmployeeId=1};

            var payload = new Payload<DashboardCheckInResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetDashboardCheckInDetails(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
