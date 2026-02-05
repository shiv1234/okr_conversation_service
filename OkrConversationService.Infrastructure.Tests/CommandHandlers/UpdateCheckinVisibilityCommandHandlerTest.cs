using Moq;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.CommandHandlers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.Adapters.CommandHandlers
{
    public class UpdateCheckinVisibilityCommandHandlerTest
    {
        [Fact]
        public async Task CheckInCreateCommandHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<ICheckInService>();
            var handler = new UpdateCheckinVisibilityCommandHandler(mockService.Object);
            var command = new UpdateCheckinVisibilityCommand();

            var payload = new Payload<CheckInVisible>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.UpdateCheckinVisibility(It.IsAny<UpdateCheckinVisibilityCommand>())).Returns(Task.FromResult(payload));

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
            var handler = new UpdateCheckinVisibilityCommandHandler(mockService.Object);
            var command = new UpdateCheckinVisibilityCommand();

            var payload = new Payload<CheckInVisible>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.UpdateCheckinVisibility(It.IsAny<UpdateCheckinVisibilityCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
