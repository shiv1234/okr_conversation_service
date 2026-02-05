using Moq;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.CommandHandlers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.Adapters.CommandHandlers
{
    public class ImportPastTaskCommandHandlerTest
    {

        [Fact]
        public async Task ImportPastTaskCommandHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<ICheckInService>();
            var handler = new ImportPastTaskHandler(mockService.Object);
            var command = new ImportPastTaskCommand();

            var payload = new Payload<bool>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.ImportPastTask(It.IsAny<ImportPastTaskCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task ImportPastTaskCommandHandler_IsSuccessTrue()
        {   //Arrange
            var mockService = new Mock<ICheckInService>();
            var handler = new ImportPastTaskHandler(mockService.Object);
            var command = new ImportPastTaskCommand();

            var payload = new Payload<bool>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.ImportPastTask(It.IsAny<ImportPastTaskCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
