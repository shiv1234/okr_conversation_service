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
    public class RecognitionCategoryQueryHandlerTest
    {

        [Fact]
        public async Task RecognitionCategoryQueryHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new RecognitionCategoryQueryHandler(mockService.Object);
            var query = new RecognitionCategoryGetQuery() { };

            var payload = new Payload<RecognitionCategoryResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetCategory(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task RecognitionCategoryQueryHandler_IsSuccessTrue()
        {    //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new RecognitionCategoryQueryHandler(mockService.Object);
            var query = new RecognitionCategoryGetQuery() { };

            var payload = new Payload<RecognitionCategoryResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetCategory(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
