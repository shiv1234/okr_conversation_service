using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class IsNoteEmployeeTagQueryHandler : IRequestHandler<IsNoteEmployeeTagQuery, Payload<bool>>
    {
        private readonly INoteService _noteService;
        public IsNoteEmployeeTagQueryHandler(INoteService noteService)
        {
            _noteService = noteService;
        }
        public async Task<Payload<bool>> Handle(IsNoteEmployeeTagQuery request, CancellationToken cancellationToken)
        {
            return await _noteService.IsEmployeeTag(request.NoteId);
        }
    }
}
