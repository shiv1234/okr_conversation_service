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
    public class RecognitionByTeamIdGetQueryHandlerTest
    {
        [Fact]
        public async Task RecognitionByTeamIdGetQueryHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new RecognitionByTeamIdGetQueryHandler(mockService.Object);
            var query = new EmployeesLeaderBoardGetQuery()
            {
                Request= new Domain.RequestModel.EmployeesLeaderBoardRequest { }
            };

            var payload = new Payload<RecognitionByTeamIdResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.EmployeesLeaderBoard(query)).Returns(Task.FromResult(payload));

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
            var handler = new RecognitionByTeamIdGetQueryHandler(mockService.Object);
            var query = new EmployeesLeaderBoardGetQuery()
            {
                Request = new Domain.RequestModel.EmployeesLeaderBoardRequest { }
            };


            var payload = new Payload<RecognitionByTeamIdResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.EmployeesLeaderBoard(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
