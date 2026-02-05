using FluentValidation.TestHelper;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.Validator;
using System.Collections.Generic;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class NoteEditValidatorTest
    {
        private readonly NoteEditValidator _validator;
        public NoteEditValidatorTest()
        {
            _validator = new NoteEditValidator();
        }
        [Fact]
        public void NoteEditRequestValidator_Success()
        {
            var employeeTags = new List<NoteEmployeeTags> { new NoteEmployeeTags
            {
                    EmployeeId=1,
             }};
            var assignedFiles = new List<NoteFiles> { new NoteFiles
            {
                    StorageFileName="Test",
                    FileName="Test",
                   FilePath="Test"
            }};
            var model = new NoteEditRequest() { Description = "test", assignedFiles = assignedFiles, employeeTags = employeeTags };

            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void NoteEditRequestValidator_Failure()
        {

            var employeeTags = new List<NoteEmployeeTags> { new NoteEmployeeTags
            {
                    EmployeeId=1,
             }};
            var assignedFiles = new List<NoteFiles> { new NoteFiles
            {
                    StorageFileName="Test",
                    FileName="Test",
                   FilePath="Test"
            }};
            var model = new NoteEditRequest() { Description = "", assignedFiles = assignedFiles, employeeTags = employeeTags };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.Description);
            Assert.False(result.IsValid);
        }
    }
}
