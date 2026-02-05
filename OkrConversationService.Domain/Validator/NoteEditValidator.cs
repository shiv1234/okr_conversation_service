using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;

namespace OkrConversationService.Domain.Validator
{
    public class NoteEditValidator : AbstractValidator<NoteEditRequest>
    {
        public NoteEditValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage(ResourceMessage.Required);
            RuleForEach(x => x.assignedFiles).SetValidator(new NoteFilesRequestValidator());
            RuleForEach(x => x.employeeTags).SetValidator(new NoteAssignUserRequestValidator());
        }
    }
}
