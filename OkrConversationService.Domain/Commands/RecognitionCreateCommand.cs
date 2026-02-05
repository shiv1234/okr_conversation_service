using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.Commands
{
    public class RecognitionCreateCommand : BaseCommand, IRequest<Payload<RecognitionCreateRequest>>
    {
        public RecognitionCreateRequest RecognitionRequest { get; set; }
    }
}
