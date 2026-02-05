using FluentValidation.TestHelper;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.Validator;
using System.Collections.Generic;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class ConversationEditValidatorTest
    {
        private readonly ConversationEditValidator _validator;
        public ConversationEditValidatorTest()
        {
            _validator = new ConversationEditValidator();
        }
        [Fact]
        public void ConversationEditValidator_Success()
        {
            var model = new ConversationEditRequest { Description = "abc", assignedFiles = new List<ConversationFiles>() , Type = 1 , employeeTags = new List<ConversationEmployeeTags>() , ConversationId = 1 , IsActive = true};
            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ConversationEditValidator_Failure()
        {
            var model = new ConversationEditRequest { Description = string.Empty};
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.Description);
            Assert.False(result.IsValid);
        }
    }
}
