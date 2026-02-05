using FluentValidation;
using OkrConversationService.Domain.Common;

namespace OkrConversationService.Domain.Validator
{
    public class UpdateCheckinVisibilityValidator : AbstractValidator<CheckInVisible>
    {
        public UpdateCheckinVisibilityValidator()
        {
            RuleFor(x => x).IsInEnum().WithMessage(ResourceMessage.InvalidCheckInVisibilty);
        }
    }
}
