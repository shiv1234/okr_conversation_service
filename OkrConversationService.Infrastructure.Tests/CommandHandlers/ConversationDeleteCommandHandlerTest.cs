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
    public class ConversationDeleteCommandHandlerTest
    {
        [Fact]
        public async Task ConversationDeleteCommandHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new ConversationDeleteCommandHandler(mockService.Object);
            var command = new ConversationDeleteCommand();

            var payload = new Payload<bool>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.DeleteConversation(It.IsAny<ConversationDeleteCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }
        [Fact]
        public async Task TaskCreateHandler_IsSuccessTrue()
        {   //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new ConversationDeleteCommandHandler(mockService.Object);
            var command = new ConversationDeleteCommand();

            var payload = new Payload<bool>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.DeleteConversation(It.IsAny<ConversationDeleteCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
