using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class GetRecognitionByIdQueryHandler : IRequestHandler<GetRecognitionByIdQuery, Payload<RecognitionResponse>>
    {
        private readonly IRecognitionService _recognitionService;
        public GetRecognitionByIdQueryHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<RecognitionResponse>> Handle(GetRecognitionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.GetRecognitionById(request);
        }
    }
}
