using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
  
    public class GetRecognitionForWallQueryTest : BaseTest
    {
        [Fact]
        public void GetRecognitionForWallQuery_Success()
        {
            var model = new GetRecognitionForWallQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
