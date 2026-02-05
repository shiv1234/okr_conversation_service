using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;
using System.Collections.Generic;

namespace OkrConversationService.Domain.Validator
{
    public class AddEmployeeCheckInVisibleValidator : AbstractValidator<EmployeeCheckInVisibleRequest>
    {
        public AddEmployeeCheckInVisibleValidator()
        {
           
        }

    }
}
