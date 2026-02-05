using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OkrConversationService.Domain.ResponseModels;
using System.Collections.Generic;

namespace OkrConversationService.Domain.Ports
{
    public interface ICommonBase
    {
        Payload<T> GetPayloadStatus<T>(Payload<T> payload, ModelStateDictionary modelState);
        Payload<T> GetPayloadStatus<T>(Payload<T> payload, List<ValidationFailure> errors);
    }
}
