using FluentValidation.TestHelper;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.Validator;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class CommentCreateValidatorTest
    {

        private readonly CommentCreateValidator _validator;
        public CommentCreateValidatorTest()
        {
            _validator = new CommentCreateValidator();
        }
        [Fact]
        public void CommentCreateValidator_Success()
        {
            var model = new CommentDetailsRequest {  CommentDetailsId=1, Comments="test" };
            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void CommentCreateValidator_Failure()
        {
            var model = new CommentDetailsRequest { CommentDetailsId = 1, Comments = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.Comments);
            Assert.False(result.IsValid);
        }
    }
}
