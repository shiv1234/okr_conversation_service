using Moq;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Adapters.QueryHandlers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.Adapters.QueryHandlers
{
    public class AllDirectReportsEmployeeByIdQueryHandlerTest
    {
        [Fact]
        public async Task AllDirectReportsEmployeeByIdQueryHandler_IsSuccessFalse()
        {
            //Arrange   

            var mockService = new Mock<ICheckInService>();
            var handler = new AllDirectReportsEmployeeByIdHandler(mockService.Object);
            var command = new AllDirectReportsEmployeeByIdQuery() { EmpId = 1 };

            var payload = new Payload<DirectreportsResponseResult>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetAllDirectReportsByIds(command)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(command, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task AllDirectReportsEmployeeByIdQueryHandler_IsSuccessTrue()
        {    //Arrange               

            var mockService = new Mock<ICheckInService>();
            var handler = new AllDirectReportsEmployeeByIdHandler(mockService.Object);
            var query = new AllDirectReportsEmployeeByIdQuery() { EmpId = 1 };

            var payload = new Payload<DirectreportsResponseResult>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetAllDirectReportsByIds(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
