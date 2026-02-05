using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class RecognitionDeleteCommandHandler : IRequestHandler<RecognitionDeleteCommand, Payload<bool>>
    {
        private readonly IRecognitionService _recognitionService;
        public RecognitionDeleteCommandHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<bool>> Handle(RecognitionDeleteCommand request, CancellationToken cancellationToken)
        {
            return await _recognitionService.Delete(request);
        }
    }
}
