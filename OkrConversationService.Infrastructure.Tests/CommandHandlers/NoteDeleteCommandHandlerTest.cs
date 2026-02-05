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
    public class NoteDeleteCommandHandlerTest
    {
        [Fact]
        public async Task NoteDeleteCommandHandler_IsSuccessTrue()
        {   //Arrange
            var mockService = new Mock<INoteService>();
            var handler = new NoteDeleteCommandHandler(mockService.Object);
            var command = new NoteDeleteCommand();

            var payload = new Payload<long>();

            mockService.Setup(c => c.DeleteNote(It.IsAny<NoteDeleteCommand>())).ReturnsAsync(payload);

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
