using Moq;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.CommandHandlers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.Adapters.CommandHandlers
{
    public class CheckInCreateCommandHandlerTest
    {
        [Fact]
        public async Task CheckInCreateCommandHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<ICheckInService>();
            var handler = new CheckInCreateCommandHandler(mockService.Object);
            var command = new CheckInCreateCommand();

            var payload = new Payload<CheckInDetailRequest>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.Create(It.IsAny<CheckInCreateCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CheckInCreateCommandHandler_IsSuccessTrue()
        {   //Arrange
            var mockService = new Mock<ICheckInService>();
            var handler = new CheckInCreateCommandHandler(mockService.Object);
            var command = new CheckInCreateCommand();

            var payload = new Payload<CheckInDetailRequest>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.Create(It.IsAny<CheckInCreateCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
