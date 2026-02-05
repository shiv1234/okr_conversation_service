using Moq;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.QueryHandlers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.Adapters.QueryHandlers
{
    public class GetAllCheckInQueryHandlerTest
    {
        [Fact]
        public async Task CheckInGetAllQueryHandler_IsSuccessFalse()
        {
            //Arrange   

            var mockService = new Mock<ICheckInService>();
            var handler = new CheckInGetAllQueryHandler(mockService.Object);
            var command = new CheckInGetAllQuery() { StartDate = DateTime.Now.Date, EndDate = DateTime.Now.Date, EmployeeId = 1 };

            var payload = new Payload<CheckInPointsResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetAll(command)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CheckInGetAllQueryHandler_IsSuccessTrue()
        {    //Arrange               

            var mockService = new Mock<ICheckInService>();
            var handler = new CheckInGetAllQueryHandler(mockService.Object);
            var query = new CheckInGetAllQuery() { StartDate = DateTime.Now.Date, EndDate = DateTime.Now.Date, EmployeeId = 1 };

            var payload = new Payload<CheckInPointsResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetAll(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
