using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;

namespace OkrConversationService.Domain.Validator
{
    public class ConversationFilesRequestValidator : AbstractValidator<ConversationFiles>
    {
        public ConversationFilesRequestValidator()
        {
            RuleFor(x => x.FileName).NotEmpty().WithMessage(ResourceMessage.Required);
            RuleFor(x => x.FilePath).NotEmpty().WithMessage(ResourceMessage.Required);
            RuleFor(x => x.StorageFileName).NotEmpty().WithMessage(ResourceMessage.Required);
        }
    }
}