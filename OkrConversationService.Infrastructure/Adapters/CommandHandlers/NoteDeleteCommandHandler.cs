using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class NoteDeleteCommandHandler : IRequestHandler<NoteDeleteCommand, Payload<long>>
    {
        private readonly INoteService _noteService;
        public NoteDeleteCommandHandler(INoteService noteService)
        {
            _noteService = noteService;
        }

        public async Task<Payload<long>> Handle(NoteDeleteCommand request, CancellationToken cancellationToken)
        {
            return await _noteService.DeleteNote(request);
        }
    }
}
