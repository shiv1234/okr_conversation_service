using Moq;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.QueryHandlers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
namespace OkrConversationService.Infrastructure.Tests.Adapters.QueryHandlers
{
    public class CheckInWeeklyDatesQueryHandlerTest
    {
        [Fact]
        public async Task CheckInWeeklyDatesQueryHandler_IsSuccessFalse()
        {
            //Arrange   

            var mockService = new Mock<ICheckInService>();
            var handler = new CheckInWeeklyDatesQueryHandler(mockService.Object);
            var command = new CheckInWeeklyDatesQuery() { EmployeeId = 1 };

            var payload = new Payload<CheckInDatesPermissionResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetAllCheckInWeeklyDates(command)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CheckInWeeklyDatesQueryHandler_IsSuccessTrue()
        {    //Arrange               

            var mockService = new Mock<ICheckInService>();
            var handler = new CheckInWeeklyDatesQueryHandler(mockService.Object);
            var query = new CheckInWeeklyDatesQuery() { EmployeeId = 1 };

            var payload = new Payload<CheckInDatesPermissionResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetAllCheckInWeeklyDates(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
