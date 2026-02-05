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
    public class GetRecognitionForWallQueryHandlerTest
    {
        [Fact]
        public async Task GetRecognitionForWallQueryHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new GetRecognitionForWallQueryHandler(mockService.Object);
            var query = new GetRecognitionForWallQuery()
            {
               emailId="support@unlockokr.com"
            };

            var payload = new Payload<RecognitionDetailsResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetRecognition(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task RecognitionByTeamIdGetQueryHandler_IsSuccessTrue()
        {    //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new GetRecognitionForWallQueryHandler(mockService.Object);
            var query = new GetRecognitionForWallQuery()
            {
                emailId = "support@unlockokr.com"
            };


            var payload = new Payload<RecognitionDetailsResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetRecognition(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
