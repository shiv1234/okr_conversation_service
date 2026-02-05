using OkrConversationService.Domain.Queries;
using Xunit;

namespace OkrConversationService.Domain.Tests.Queries
{
    public class RecognitionCategoryGetQueryTest : BaseTest
    {
        [Fact]
        public void RecognitionCategoryGetQuery_Success()
        {
            var model = new RecognitionCategoryGetQuery();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
