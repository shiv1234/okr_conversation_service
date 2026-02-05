using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class TotalRecognitionByTeamIdGetQueryHandler : IRequestHandler<TotalRecognitionByTeamIdGetQuery, Payload<TotalRecognitionByTeamIdResponse>>
    {

        private readonly IRecognitionService _recognitionService;
        public TotalRecognitionByTeamIdGetQueryHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<TotalRecognitionByTeamIdResponse>> Handle(TotalRecognitionByTeamIdGetQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.TotalRecognitionByTeamId(request);
        }
    }
}
