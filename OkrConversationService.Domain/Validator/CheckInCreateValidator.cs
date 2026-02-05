using FluentValidation;
using OkrConversationService.Domain.RequestModel;
using System.Collections.Generic;

namespace OkrConversationService.Domain.Validator
{
    public class CheckInCreateValidator : AbstractValidator<List<CheckInDetailRequest>>
    {
        public CheckInCreateValidator()
        {

        }
    }
}
