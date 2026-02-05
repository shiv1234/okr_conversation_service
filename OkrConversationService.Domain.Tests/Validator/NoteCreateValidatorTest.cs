using FluentValidation.TestHelper;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.Validator;
using System.Collections.Generic;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{
    public class NoteCreateValidatorTest
    {
        private readonly NoteCreateValidator _validator;
        public NoteCreateValidatorTest()
        {
            _validator = new NoteCreateValidator();
        }
        [Fact]
        public void NoteCreateRequestValidator_Success()
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
            var model = new NoteCreateRequest() { Description = "test", GoalTypeId = 498, GoalId = 1, assignedFiles = assignedFiles, employeeTags = employeeTags };

            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void NoteCreateRequestValidator_Failure()
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
            var model = new NoteCreateRequest() { Description = "test", GoalTypeId = 0, GoalId = 1, assignedFiles = assignedFiles, employeeTags = employeeTags };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.GoalTypeId);
            Assert.False(result.IsValid);
        }
    }
}
