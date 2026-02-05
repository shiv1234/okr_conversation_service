using FluentValidation;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;

namespace OkrConversationService.Domain.Validator
{
    public class DeleteConversationValidator : AbstractValidator<ConversationDeleteCommand>
    {
        public DeleteConversationValidator()
        {
            RuleFor(x => x.ConversationId).GreaterThan(0).WithMessage(ResourceMessage.Required);
        }
    }
}
