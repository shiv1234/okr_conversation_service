using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class NoteEditHandler : IRequestHandler<NoteEditCommand, Payload<NoteEditRequest>>
    {
        private readonly INoteService _noteService;

        public NoteEditHandler(INoteService noteService)
        {
            _noteService = noteService;
        }

        public async Task<Payload<NoteEditRequest>> Handle(NoteEditCommand request,
            CancellationToken cancellationToken)
        {
            return await _noteService.Edit(request);
        }
    }
}
