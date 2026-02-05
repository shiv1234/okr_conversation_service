using FluentValidation;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;

namespace OkrConversationService.Domain.Validator
{
    public class DeleteNoteValidator : AbstractValidator<NoteDeleteCommand>
    {
        public DeleteNoteValidator()
        {
            RuleFor(x => x.NoteId).GreaterThan(0).WithMessage(ResourceMessage.Required);
        }
    }
}
