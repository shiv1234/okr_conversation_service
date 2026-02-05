using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class MyWallOfFameResponseTest : BaseTest
    {
        [Fact]
        public void MyWallOfFameResponse_Success()
        {
            var model = new MyWallOfFameResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void RecognitionImageMappingResponse_Success()
        {
            var model = new RecognitionImageMappingResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void RecognitionUserDetailsResponse_Success()
        {
            var model = new RecognitionUserDetailsResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void RecognitionTeamDetailResponse_Success()
        {
            var model = new RecognitionTeamDetailResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        

    }
}
