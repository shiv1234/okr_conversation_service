using OkrConversationService.Domain.ResponseModels;
using Xunit;

namespace OkrConversationService.Domain.Tests.ResponseModels
{
    public class NoteEmployeeTagResponseTest : BaseTest
    {
        [Fact]
        public void NoteEmployeeTagResponse_Success()
        {
            var model = new NoteEmployeeTagResponse();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
