using Moq;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.CommandHandlers;
using OkrConversationService.Infrastructure.Adapters.CommandHandlers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.CommandHandlers
{
    public class NoteUploadFileCommandHandlerTest
    {
        [Fact]
        public async Task NoteUploadFileCommandHandlerTest_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<INoteService>();
            var handler = new UploadNoteFileCommandHandler(mockService.Object);
            var command = new UploadFileCommand();

            var payload = new Payload<string>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.UploadNotesImageOnAzure(It.IsAny<UploadFileCommand>())).Returns(Task.FromResult(payload));

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
            var mockService = new Mock<INoteService>();
            var handler = new UploadNoteFileCommandHandler(mockService.Object);
            var command = new UploadFileCommand();

            var payload = new Payload<string>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.UploadNotesImageOnAzure(It.IsAny<UploadFileCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

    }
}
