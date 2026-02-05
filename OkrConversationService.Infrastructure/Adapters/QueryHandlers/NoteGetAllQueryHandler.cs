using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class NoteGetAllQueryHandler : IRequestHandler<TeamGetAllQuery, Payload<NoteResponse>>
    {

        private readonly INoteService _teamService;
        public NoteGetAllQueryHandler(INoteService teamService)
        {
            _teamService = teamService;
        }
        public async Task<Payload<NoteResponse>> Handle(TeamGetAllQuery request, CancellationToken cancellationToken)
        {
            return await _teamService.GetAll(request);
        }
    }
}
