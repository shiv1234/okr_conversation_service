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
    public class CommentDeleteCommandHandlerTest
    {
        [Fact]
        public async Task CommentDeleteCommandHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new CommentDeleteHandler(mockService.Object);
            var command = new CommentDeleteCommand();

            var payload = new Payload<bool>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.DeleteComment(It.IsAny<CommentDeleteCommand>()))
                .Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CommentDeleteCommandHandler_IsSuccessTrue()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new CommentDeleteHandler(mockService.Object);
            var command = new CommentDeleteCommand();

            var payload = new Payload<bool>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.DeleteComment(It.IsAny<CommentDeleteCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

    }
}
