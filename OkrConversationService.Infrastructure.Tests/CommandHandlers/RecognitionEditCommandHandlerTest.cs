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
    public class RecognitionEditCommandHandlerTest
    {
        [Fact]
        public async Task RecognitionEditCommandHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new RecognitionEditCommandHandler(mockService.Object);
            var command = new RecognitionEditCommand();

            var payload = new Payload<RecognitionEditRequest>()
            {
                IsSuccess = false
            };

            mockService.Setup(c => c.EditRecognition(It.IsAny<RecognitionEditCommand>()))
                .Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task RecognitionEditCommandHandler_IsSuccessTrue()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new RecognitionEditCommandHandler(mockService.Object);
            var command = new RecognitionEditCommand();

            var payload = new Payload<RecognitionEditRequest>()
            {
                IsSuccess = true
            };

            mockService.Setup(c => c.EditRecognition(It.IsAny<RecognitionEditCommand>()))
                .Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

    }
}
