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
    public class IsEmployeeTagQueryHandlerTest
    {
        [Fact]
        public async Task IsEmployeeTagQueryHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new IsEmployeeTagQueryHandler(mockService.Object);
            var query = new IsEmployeeTagQuery() { ConversationId = 1 };

            var payload = new Payload<bool>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.IsEmployeeTag(query.ConversationId)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task IsEmployeeTagQueryHandler_IsSuccessTrue()
        {    //Arrange
            var mockService = new Mock<IConversationService>();
            var handler = new IsEmployeeTagQueryHandler(mockService.Object);
            var query = new IsEmployeeTagQuery() { ConversationId = 1 };

            var payload = new Payload<bool>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.IsEmployeeTag(query.ConversationId)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }
    }
}
