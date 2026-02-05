using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;

namespace OkrConversationService.Domain.Validator
{
    public class NoteAssignUserRequestValidator : AbstractValidator<NoteEmployeeTags>
    {
        public NoteAssignUserRequestValidator()
        {
            RuleFor(x => x.EmployeeId).NotNull().WithMessage(ResourceMessage.Required).GreaterThan(0).WithMessage(ResourceMessage.Required);
        }
    }
}