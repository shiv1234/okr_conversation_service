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
    public class TeamsLeaderBoardGetQueryHandlerTest
    {
        [Fact]
        public async Task TeamsLeaderBoardGetQueryHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new TeamsLeaderBoardGetQueryHandler(mockService.Object);
            var query = new TeamsLeaderBoardGetQuery()
            {
                Request = new
                Domain.RequestModel.RecognitionLeaderBoardRequest
                {  Id = 1, SearchType = 0 }
            };

            var payload = new Payload<RecognitionTeamsResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.TeamsLeaderBoard(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task TeamsLeaderBoardGetQueryHandler_IsSuccessTrue()
        {    //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new TeamsLeaderBoardGetQueryHandler(mockService.Object);
            var query = new TeamsLeaderBoardGetQuery()
            {
                Request = new
                   Domain.RequestModel.RecognitionLeaderBoardRequest
                {  Id = 1, SearchType = 0 }
            };

            var payload = new Payload<RecognitionTeamsResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.TeamsLeaderBoard(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
