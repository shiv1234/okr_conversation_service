using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;

namespace OkrConversationService.Domain.Validator
{
    public class RecognitionCreateValidator : AbstractValidator<RecognitionCreateRequest>
    {
        public RecognitionCreateValidator()
        {
         
            RuleFor(x => x.Message).NotEmpty().WithMessage(ResourceMessage.Required);
        }

    }
}
