using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.Validator
{
    public class RecognitionEditValidator : AbstractValidator<RecognitionEditRequest>
    {
        public RecognitionEditValidator()
        {
           
            RuleFor(x => x.Message).NotEmpty().WithMessage(ResourceMessage.Required);
            RuleFor(x => x.RecognitionId).GreaterThan(0).WithMessage(ResourceMessage.Required);
        }
    }
}
