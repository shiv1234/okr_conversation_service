using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class TeamsByEmpIdGetQueryHandler : IRequestHandler<TeamsByEmpIdGetQuery, Payload<TeamByEmpIdResponse>>
    {

        private readonly IRecognitionService _recognitionService;
        public TeamsByEmpIdGetQueryHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<TeamByEmpIdResponse>> Handle(TeamsByEmpIdGetQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.GetTeamsByEmpId(request);
        }
    }
}
