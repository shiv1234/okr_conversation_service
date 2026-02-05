using FluentValidation.TestHelper;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.Validator;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class AssignUserRequestValidatorTest
    {
        private readonly AssignUserRequestValidator _validator;
        public AssignUserRequestValidatorTest()
        {
            _validator = new AssignUserRequestValidator();
        }
        [Fact]
        public void ConversationAssignUserRequestValidator_Success()
        {
            var model = new ConversationEmployeeTags { EmployeeId = 1 };
            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ConversationAssignUserRequestValidator_Failure()
        {
            var model = new ConversationEmployeeTags { EmployeeId = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.EmployeeId);
            Assert.False(result.IsValid);
        }
    }
}
