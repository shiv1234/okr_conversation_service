using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public  class TotalRecognitionByTeamIdGetQueryTest : BaseTest
    {
        [Fact]
        public void TotalRecognitionByTeamIdGetQuery_Success()
        {
            var model = new TotalRecognitionByTeamIdGetQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
