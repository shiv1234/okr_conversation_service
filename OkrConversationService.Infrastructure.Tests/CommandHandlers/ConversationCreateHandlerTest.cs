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
    public class ConversationCreateHandlerTest
    {
        [Fact]
        public async Task ConversationCreateHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new ConversationCreateHandler(mockService.Object);
            var command = new ConversationCreateCommand();

            var payload = new Payload<ConversationCreateRequest>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.Create(It.IsAny<ConversationCreateCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task ConversationCreateHandler_IsSuccessTrue()
        {   //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new ConversationCreateHandler(mockService.Object);
            var command = new ConversationCreateCommand();

            var payload = new Payload<ConversationCreateRequest>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.Create(It.IsAny<ConversationCreateCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

    }
}
