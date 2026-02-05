using Moq;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.CommandHandlers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.CommandHandlers
{
    public class UploadFileCommandHandlerTest
    {
        [Fact]
        public async Task UploadFileCommandHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new UploadFileCommandHandler(mockService.Object);
            var command = new UploadFileCommand();

            var payload = new Payload<string>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.UploadConversationImageOnAzure(It.IsAny<UploadFileCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task UploadFileCommandHandler_IsSuccessTrue()
        {    //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new UploadFileCommandHandler(mockService.Object);
            var command = new UploadFileCommand();

            var payload = new Payload<string>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.UploadConversationImageOnAzure(It.IsAny<UploadFileCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

    }
}
