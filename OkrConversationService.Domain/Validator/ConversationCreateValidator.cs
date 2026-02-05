using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;

namespace OkrConversationService.Domain.Validator
{
    public class ConversationCreateValidator : AbstractValidator<ConversationCreateRequest>
    {
        public ConversationCreateValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage(ResourceMessage.Required);
            RuleFor(x => x.GoalId).GreaterThan(0).WithMessage(ResourceMessage.Required);
            RuleFor(x => x.GoalTypeId).GreaterThan(0).WithMessage(ResourceMessage.Required);
            RuleFor(x => x.Type).GreaterThan(0).WithMessage(ResourceMessage.Required);
            RuleForEach(x => x.assignedFiles).SetValidator(new ConversationFilesRequestValidator());
            RuleForEach(x => x.employeeTags).SetValidator(new AssignUserRequestValidator());
            RuleFor(x => x.GoalSourceId).GreaterThan(0).WithMessage(ResourceMessage.Required);
        }
    }
}