using Moq;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.QueryHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.QueryHandlers
{
    public class GetRecognitionByIdQueryHandlerTest
    {
        [Fact]
        public async Task GetRecognitionByIdQueryHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new GetRecognitionByIdQueryHandler(mockService.Object);
            var query = new GetRecognitionByIdQuery() { };

            var payload = new Payload<RecognitionResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetRecognitionById(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetOrgRecognitionQueryHandler_IsSuccessTrue()
        {    //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new GetRecognitionByIdQueryHandler(mockService.Object);
            var query = new GetRecognitionByIdQuery() { };

            var payload = new Payload<RecognitionResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetRecognitionById(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
