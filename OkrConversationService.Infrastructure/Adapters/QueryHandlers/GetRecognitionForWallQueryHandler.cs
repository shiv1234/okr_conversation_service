using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class GetRecognitionForWallQueryHandler : IRequestHandler<GetRecognitionForWallQuery, Payload<RecognitionDetailsResponse>>
    {
        private readonly IRecognitionService _recognitionService;
        public GetRecognitionForWallQueryHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<RecognitionDetailsResponse>> Handle(GetRecognitionForWallQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.GetRecognition(request);
        }
    }
}
