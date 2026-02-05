using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
   public class RecognitionByTeamIdEmployeeTest : BaseTest
    {
        [Fact]
        public void RecognitionByTeamIdEmployee_Success()
        {
            var model = new RecognitionByTeamIdEmployee();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void RecognitionByTeamIdResponse_Success()
        {
            var model = new RecognitionByTeamIdResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void RecognitionTeam_Success()
        {
            var model = new RecognitionTeam();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        
    }
}
