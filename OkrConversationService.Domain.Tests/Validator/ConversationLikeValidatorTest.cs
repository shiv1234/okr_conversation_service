using FluentValidation.TestHelper;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.Validator;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class ConversationLikeValidatorTest
    {
        private readonly ConversationLikeValidator _validator;
        public ConversationLikeValidatorTest()
        {
            _validator = new ConversationLikeValidator();
        }
        [Fact]
        public void ConversationLikeValidator_Success()
        {
            var model = new ConversationLikeCreateRequest { ModuleDetailsId = 1 , EmployeeId = 1 , IsActive = true,ModuleId = 1};
            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ConversationLikeValidator_Failure()
        {
            var model = new ConversationLikeCreateRequest { ModuleDetailsId = 0};
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.ModuleDetailsId);
            Assert.False(result.IsValid);
        }
    }
}
