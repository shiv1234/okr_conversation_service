using FluentValidation.TestHelper;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.Validator;
using System.Collections.Generic;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class ConversationCreateValidatorTest
    {
        private readonly ConversationCreateValidator _validator;
        public ConversationCreateValidatorTest()
        {
            _validator = new ConversationCreateValidator();
        }
        [Fact]
        public void ConversationCreateValidator_Success()
        {
            var model = new ConversationCreateRequest { Description = "test", GoalTypeId = 498, GoalId = 1, assignedFiles = new List<ConversationFiles>(), ConversationId = 1, employeeTags = new List<ConversationEmployeeTags>(), GoalSourceId = 1, Type = 1 };
            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ConversationCreateValidator_Failure()
        {
            var model = new ConversationCreateRequest { Description = ""};
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.Description);
            Assert.False(result.IsValid);
        }
    }
}
