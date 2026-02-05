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
    public class IsCheckInSubmittedQueryHandlerTest
    {
        [Fact]
        public async Task IsCheckInSubmittedQueryHandler_IsSuccessFalse()
        {
            //Arrange   

            var mockService = new Mock<ICheckInService>();
            var handler = new IsCheckInSubmittedQueryHandler(mockService.Object);
            var command = new IsCheckInSubmittedQuery() { };

            var payload = new Payload<CheckInAlertResponse>()
            {
                IsSuccess = false,
                Entity = new CheckInAlertResponse() { IsAlert = false, RemainingDays = "" }

            };
            mockService.Setup(c => c.IsCheckInSubmitted()).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.Entity.IsAlert);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task IsCheckInSubmittedQueryHandler_IsSuccessTrue()
        {    //Arrange               

            //Arrange   

            var mockService = new Mock<ICheckInService>();
            var handler = new IsCheckInSubmittedQueryHandler(mockService.Object);
            var command = new IsCheckInSubmittedQuery() { };

            var payload = new Payload<CheckInAlertResponse>()
            {
                IsSuccess = true,
                Entity = new CheckInAlertResponse() { IsAlert = true, RemainingDays = "" }
            };
            mockService.Setup(c => c.IsCheckInSubmitted()).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Entity.IsAlert);
            Assert.True(result.IsSuccess);
        }
    }
}
