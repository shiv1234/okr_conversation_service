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
    public class MyWallOfFameGetQueryHandlerTest
    {
        [Fact]
        public async Task MyWallOfFameGetQueryHandler_IsSuccessFalse()
        {   //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new MyWallOfFameGetQueryHandler(mockService.Object);

            var query = new MyWallOfFameGetQuery() { MyMyWallOfFameRequest 
                =new Domain.RequestModel.MyWallOfFameRequest { Id = 1, PageIndex = 1, PageSize = 100 } };

            var payload = new Payload<MyWallOfFameResponse>()
            {
                IsSuccess = false
            };
            mockService.Setup(c => c.GetMyWallOfFameGetQuery(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task MyWallOfFameGetQueryHandler_IsSuccessTrue()
        {    //Arrange
            var mockService = new Mock<IRecognitionService>();
            var handler = new MyWallOfFameGetQueryHandler(mockService.Object);
            var query = new MyWallOfFameGetQuery()
            {
                MyMyWallOfFameRequest
                 = new Domain.RequestModel.MyWallOfFameRequest { Id = 1, PageIndex = 1, PageSize = 100 }
            };

            var payload = new Payload<MyWallOfFameResponse>()
            {
                IsSuccess = true
            };
            mockService.Setup(c => c.GetMyWallOfFameGetQuery(query)).Returns(Task.FromResult(payload));

            //Act
            var cancellationToken = new CancellationToken();
            var result = await handler.Handle(query, cancellationToken);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
