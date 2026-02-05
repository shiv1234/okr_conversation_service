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
    public class GetAllUnreadConversationQueryHandlerTest
    {
        [Fact]
        public async Task GetAllUnreadConversationQueryHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new GetAllUnreadConversationQueryHandler(mockService.Object);
            var query = new GetAllUnreadConversationQuery() { EmpId  = 1 };

            var payload = new Payload<UnreadConversationResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetAllUnreadConversation(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetAllUnreadConversationQueryHandler_IsSuccessTrue()
        {    //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new ConversationGetAllQueryHandler(mockService.Object);
            var query = new ConversationGetAllQuery() { GoalSourceId = 1 };

            var payload = new Payload<ConversationResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetAll(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
