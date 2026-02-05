using Moq;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.CommandHandlers
{
    public class CommentCreateCommandHandlerTest
    {
        [Fact]
        public async Task CommentCreateCommandHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new CommentCreateCommandHandler(mockService.Object);
            var command = new CommentCreateCommand();

            var payload = new Payload<CommentDetailsRequest>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.CreateComments(It.IsAny<CommentCreateCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CommentCreateCommandHandler_IsSuccessTrue()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new CommentCreateCommandHandler(mockService.Object);
            var command = new CommentCreateCommand();

            var payload = new Payload<CommentDetailsRequest>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.CreateComments(It.IsAny<CommentCreateCommand>())).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

    }
}
