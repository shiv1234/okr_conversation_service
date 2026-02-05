using FluentValidation.TestHelper;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Validator;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class UpdateCheckinVisibilityValidatorTest
    {
        private readonly UpdateCheckinVisibilityValidator _validator;
        public UpdateCheckinVisibilityValidatorTest()
        {
            _validator = new UpdateCheckinVisibilityValidator();
        }
        [Fact]
        public void UpdateCheckinVisibilityValidator_Success()
        {
            var result = _validator.TestValidate((CheckInVisible)1);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void UpdateCheckinVisibilityValidator_Failure()
        {
            var result = _validator.TestValidate((CheckInVisible)4);
            result.ShouldHaveValidationErrorFor(u => u);
            Assert.False(result.IsValid);
        }
    }
}
