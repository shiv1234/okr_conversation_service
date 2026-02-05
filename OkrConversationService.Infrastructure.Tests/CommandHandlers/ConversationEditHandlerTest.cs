using Moq;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.CommandHandlers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.CommandHandlers
{
    public class ConversationEditHandlerTest
    {
        [Fact]
        public async Task ConversationEditHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new ConversationEditHandler(mockService.Object);
            var command = new ConversationEditCommand();

            var payload = new Payload<ConversationEditRequest>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.Edit(It.IsAny<ConversationEditCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task ConversationEditHandler_IsSuccessTrue()
        {   //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new ConversationEditHandler(mockService.Object);
            var command = new ConversationEditCommand();

            var payload = new Payload<ConversationEditRequest>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.Edit(It.IsAny<ConversationEditCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

    }
}
