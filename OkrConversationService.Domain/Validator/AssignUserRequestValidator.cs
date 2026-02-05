using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;

namespace OkrConversationService.Domain.Validator
{
    public class AssignUserRequestValidator : AbstractValidator<ConversationEmployeeTags>
    {
        public AssignUserRequestValidator()
        {
            RuleFor(x => x.EmployeeId).NotNull().WithMessage(ResourceMessage.Required).GreaterThan(0).WithMessage(ResourceMessage.Required);
        }
    }
}