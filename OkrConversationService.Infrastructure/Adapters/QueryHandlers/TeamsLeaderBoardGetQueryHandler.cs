using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class TeamsLeaderBoardGetQueryHandler : IRequestHandler<TeamsLeaderBoardGetQuery, Payload<RecognitionTeamsResponse>>
    {

        private readonly IRecognitionService _recognitionService;
        public TeamsLeaderBoardGetQueryHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<RecognitionTeamsResponse>> Handle(TeamsLeaderBoardGetQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.TeamsLeaderBoard(request);
        }
    }
}
