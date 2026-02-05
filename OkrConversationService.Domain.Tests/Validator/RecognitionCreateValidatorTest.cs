using FluentValidation.TestHelper;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.Validator;
using System.Collections.Generic;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class RecognitionCreateValidatorTest
    {
        private readonly RecognitionCreateValidator _validator;
        public RecognitionCreateValidatorTest()
        {
            _validator = new RecognitionCreateValidator();
        }
        [Fact]
        public void RecognitionCreateValidator_Success()
        {
            var model = new RecognitionCreateRequest {   IsAttachment=false, Message="test", RecognitionCategoryId=1,
             RecognitionCategoryTypeId=1, RecognitionId=1, RecognitionImageRequests= new List<RecognitionImageRequest> { new RecognitionImageRequest { FileName="", GuidFileName="" } }
            };
            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

    }
}
