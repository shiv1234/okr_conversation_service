using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;

namespace OkrConversationService.Domain.Validator
{
    public class CheckInValidator : AbstractValidator<CheckInDetailRequest>
    {
        public CheckInValidator()
        {
            RuleFor(x => x.CheckInPointsId).GreaterThan(0).WithMessage(ResourceMessage.Required);
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage(ResourceMessage.Required);
        }

    }
}
