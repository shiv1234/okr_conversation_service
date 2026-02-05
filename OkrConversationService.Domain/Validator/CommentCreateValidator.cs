using FluentValidation;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.Validator
{
    public class CommentCreateValidator : AbstractValidator<CommentDetailsRequest>
    {
        public CommentCreateValidator()
        {
            RuleFor(x => x.Comments).NotEmpty().WithMessage(ResourceMessage.Required);
        }
    }
}
