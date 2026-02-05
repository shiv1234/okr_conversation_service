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
    public class GetOrgRecognitionQueryHandlerTest
    {
        [Fact]
        public async Task GetOrgRecognitionQueryHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new GetOrgRecognitionQueryHandler(mockService.Object);
            var query = new GetOrgRecognitionQuery() {  };

            var payload = new Payload<OrgRecognitionResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetOrgRecognition(query)).Returns(Task.FromResult(payload));

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
            var handler = new GetOrgRecognitionQueryHandler(mockService.Object);
            var query = new GetOrgRecognitionQuery() {  };

            var payload = new Payload<OrgRecognitionResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetOrgRecognition(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
