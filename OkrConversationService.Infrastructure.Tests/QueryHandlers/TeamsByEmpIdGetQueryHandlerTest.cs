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
    public class TeamsByEmpIdGetQueryHandlerTest
    {
        [Fact]
        public async Task TeamsByEmpIdGetQueryHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new TeamsByEmpIdGetQueryHandler(mockService.Object);
            var query = new TeamsByEmpIdGetQuery() { };

            var payload = new Payload<TeamByEmpIdResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetTeamsByEmpId(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task TeamsByEmpIdGetQueryHandler_IsSuccessTrue()
        {    //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new TeamsByEmpIdGetQueryHandler(mockService.Object);
            var query = new TeamsByEmpIdGetQuery() { };

            var payload = new Payload<TeamByEmpIdResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetTeamsByEmpId(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
