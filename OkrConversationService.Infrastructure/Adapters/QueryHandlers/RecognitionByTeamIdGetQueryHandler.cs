using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class RecognitionByTeamIdGetQueryHandler : IRequestHandler<EmployeesLeaderBoardGetQuery, Payload<RecognitionByTeamIdResponse>>
    {

        private readonly IRecognitionService _recognitionService;
        public RecognitionByTeamIdGetQueryHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<RecognitionByTeamIdResponse>> Handle(EmployeesLeaderBoardGetQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.EmployeesLeaderBoard(request);
        }
    }
}
