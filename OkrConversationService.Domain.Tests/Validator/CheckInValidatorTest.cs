using FluentValidation.TestHelper;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.Validator;
using Xunit;

namespace OkrConversationService.Domain.Tests.Validator
{


    public class CheckInValidatorTest
    {
        private readonly CheckInValidator _validator;
        public CheckInValidatorTest()
        {
            _validator = new CheckInValidator();
        }
        [Fact]
        public void CheckInValidator_Success()
        {

            var model = new CheckInDetailRequest() { CheckInDetails = "", CheckInDetailsId = 1, CheckInPointsId = 1, EmployeeId = 1 };

            var result = _validator.TestValidate(model);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void CheckInValidatorCheckInPointsId_Failure()
        {


            var model = new CheckInDetailRequest() { CheckInDetails = "", CheckInDetailsId = 1, CheckInPointsId = 0, EmployeeId = 1 };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.CheckInPointsId);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void CheckInValidatorEmployeeId_Failure()
        {


            var model = new CheckInDetailRequest() { CheckInDetails = "", CheckInDetailsId = 1, CheckInPointsId = 1, EmployeeId = 0 };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.EmployeeId);
            Assert.False(result.IsValid);
        }
    }
}
