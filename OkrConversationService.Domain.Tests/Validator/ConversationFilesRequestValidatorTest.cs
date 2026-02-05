using FluentValidation.TestHelper;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.Validator;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class ConversationFilesRequestValidatorTest
    {
        private readonly ConversationFilesRequestValidator _validator;
        public ConversationFilesRequestValidatorTest()
        {
            _validator = new ConversationFilesRequestValidator();
        }
        [Fact]
        public void ConversationFilesRequestValidator_Success()
        {
            var model = new ConversationFiles {FileName = "test" , FilePath = "test" , StorageFileName = "test"};
            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ConversationFilesRequestValidator_Failure()
        {
            var model = new ConversationFiles { FileName = ""};
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.FileName);
            Assert.False(result.IsValid);
        }
    }
}
