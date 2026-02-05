using Moq;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.QueryHandlers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.QueryHandlers
{
    public class RecognitionLikeQueryHandlersTest
    {
        [Fact]
        public async Task GetRecognitionLikeQueryHandlers_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new RecognitionLikeQueryHandlers(mockService.Object);
            var query = new RecognitionLikeQuery() { ModuleDetailsId=1 };

            var payload = new Payload<RecognitionReactionResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetRecognitionLike(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetRecognitionLikeQueryHandlers_IsSuccessTrue()
        {    //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new RecognitionLikeQueryHandlers(mockService.Object);
            var query = new RecognitionLikeQuery() { ModuleDetailsId = 1 };

            var payload = new Payload<RecognitionReactionResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetRecognitionLike(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
