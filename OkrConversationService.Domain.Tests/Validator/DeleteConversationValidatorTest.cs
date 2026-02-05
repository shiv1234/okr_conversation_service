using FluentValidation.TestHelper;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Validator;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class DeleteConversationValidatorTest
    {
        private readonly DeleteConversationValidator _validator;
        public DeleteConversationValidatorTest()
        {
            _validator = new DeleteConversationValidator();
        }
        [Fact]
        public void DeleteConversationValidator_Success()
        {
            var ConversationId = 1;
            var model = new ConversationDeleteCommand { ConversationId = ConversationId };
            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void DeleteConversationValidator_Failure()
        {
            var ConversationId = 0;
            var model = new ConversationDeleteCommand { ConversationId = ConversationId };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.ConversationId);
            Assert.False(result.IsValid);
        }
    }
}
