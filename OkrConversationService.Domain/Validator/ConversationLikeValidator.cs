using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;

namespace OkrConversationService.Domain.Validator
{
    public class ConversationLikeValidator : AbstractValidator<ConversationLikeCreateRequest>
    {
        public ConversationLikeValidator()
        {
            RuleFor(x => x.ModuleDetailsId).GreaterThan(0).WithMessage(ResourceMessage.Required);
            RuleFor(x => x.ModuleId).GreaterThan(0).WithMessage(ResourceMessage.Required);
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage(ResourceMessage.Required);
        }
    }
}
