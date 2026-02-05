using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class RecognitionLikeQueryHandlers : IRequestHandler<RecognitionLikeQuery, Payload<RecognitionReactionResponse>>
    {
        private readonly IRecognitionService _recognitionService;
        public RecognitionLikeQueryHandlers(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<RecognitionReactionResponse>> Handle(RecognitionLikeQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.GetRecognitionLike(request);
        }
    }
}
