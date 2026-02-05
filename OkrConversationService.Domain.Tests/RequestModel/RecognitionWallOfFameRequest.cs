using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class RecognitionWallOfFameRequest : BaseTest
    {
        [Fact]
        public void RecognitionWallOfFameRequest_Success()
        {
            var model = new Domain.RequestModel.RecognitionWallOfFame();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
