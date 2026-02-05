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
    public class NoteEditCommandHandlerTest
    {
        [Fact]
        public async Task NoteEditCommandHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<INoteService>();
            var handler = new NoteEditHandler(mockService.Object);
            var command = new NoteEditCommand();

            var payload = new Payload<NoteEditRequest>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.Edit(It.IsAny<NoteEditCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task NoteEditCommandHandler_IsSuccessTrue()
        {   //Arrange
            var mockService = new Mock<INoteService>();
            var handler = new NoteEditHandler(mockService.Object);
            var command = new NoteEditCommand();

            var payload = new Payload<NoteEditRequest>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.Edit(It.IsAny<NoteEditCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
