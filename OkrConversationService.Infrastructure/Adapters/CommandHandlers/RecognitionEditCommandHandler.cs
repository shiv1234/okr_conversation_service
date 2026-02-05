using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class RecognitionEditCommandHandler : IRequestHandler<RecognitionEditCommand, Payload<RecognitionEditRequest>>
    {
        private readonly IRecognitionService _recognitionService;
        public RecognitionEditCommandHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<RecognitionEditRequest>> Handle(RecognitionEditCommand request, CancellationToken cancellationToken)
        {
            return await _recognitionService.EditRecognition(request);
        }
    }
}
