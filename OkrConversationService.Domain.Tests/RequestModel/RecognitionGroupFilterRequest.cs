using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OkrConversationService.Domain.Tests.RequestModel
{
    public class RecognitionGroupFilterRequest : BaseTest
    {
        [Fact]
        public void RecognitionGroupFilterRequest_Success()
        {
            var model = new Domain.RequestModel.RecognitionGroupFilter();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
    }
}
