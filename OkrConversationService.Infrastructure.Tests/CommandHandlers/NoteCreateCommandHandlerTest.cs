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
    public class NoteCreateCommandHandlerTest
    {
        [Fact]
        public async Task NoteCreateCommandHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<INoteService>();
            var handler = new NoteCreateHandler(mockService.Object);
            var command = new NoteCreateCommand();

            var payload = new Payload<NoteCreateRequest>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.Create(It.IsAny<NoteCreateCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task NoteCreateCommandHandler_IsSuccessTrue()
        {
            //Arrange          
            var mockService = new Mock<INoteService>();
            var handler = new NoteCreateHandler(mockService.Object);
            var command = new NoteCreateCommand();

            var payload = new Payload<NoteCreateRequest>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.Create(It.IsAny<NoteCreateCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
