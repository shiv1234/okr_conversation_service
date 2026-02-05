using FluentValidation.TestHelper;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Validator;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class RecognitionDeleteValidatorTest
    {
        private readonly RecognitionDeleteValidator _validator;
        public RecognitionDeleteValidatorTest()
        {
            _validator = new RecognitionDeleteValidator();
        }
        [Fact]
        public void RecognitionDeleteValidator_Success()
        {
            var model = new RecognitionDeleteCommand { RecognitionId=1, IsActive=true };
            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void RecognitionDeleteValidator_Failure()
        {
            var model = new RecognitionDeleteCommand { RecognitionId = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.RecognitionId);
            Assert.False(result.IsValid);
        }
    }
}
