using FluentValidation;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;


namespace OkrConversationService.Domain.Validator
{
    public class RecognitionDeleteValidator : AbstractValidator<RecognitionDeleteCommand>
    {
        public RecognitionDeleteValidator()
        {
            RuleFor(x => x.RecognitionId).GreaterThan(0).WithMessage(ResourceMessage.Required);
        }
    }
}
