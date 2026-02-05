using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;


namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
   public class UserNoteAllQueryHandler : IRequestHandler<UserNoteAllQuery, Payload<UserNoteResponse>>
    {

        private readonly INoteService _teamService;
        public UserNoteAllQueryHandler(INoteService teamService)
        {
            _teamService = teamService;
        }
        public async Task<Payload<UserNoteResponse>> Handle(UserNoteAllQuery request, CancellationToken cancellationToken)
        {
            return await _teamService.GetAllUserNotes(request);
        }
    }
}
