using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;

namespace OkrConversationService.Domain.Validator
{
    public class NoteCreateValidator : AbstractValidator<NoteCreateRequest>
    {
        public NoteCreateValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage(ResourceMessage.Required);
            RuleFor(x => x.GoalId).GreaterThan(0).WithMessage(ResourceMessage.Required);
            RuleFor(x => x.GoalTypeId).GreaterThan(0).WithMessage(ResourceMessage.Required);
            RuleForEach(x => x.assignedFiles).SetValidator(new NoteFilesRequestValidator());
            RuleForEach(x => x.employeeTags).SetValidator(new NoteAssignUserRequestValidator());
        }
    }
}
