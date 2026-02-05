using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class GetOrgRecognitionQueryHandler : IRequestHandler<GetOrgRecognitionQuery, Payload<OrgRecognitionResponse>>
    {
        private readonly IRecognitionService _recognitionService;
        public GetOrgRecognitionQueryHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<OrgRecognitionResponse>> Handle(GetOrgRecognitionQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.GetOrgRecognition(request);
        }
    }
}
