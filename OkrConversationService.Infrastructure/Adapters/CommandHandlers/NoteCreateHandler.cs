using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class NoteCreateHandler : IRequestHandler<NoteCreateCommand, Payload<NoteCreateRequest>>
    {
        private readonly INoteService _noteService;

        public NoteCreateHandler(INoteService noteService)
        {
            _noteService = noteService;
        }

        public async Task<Payload<NoteCreateRequest>> Handle(NoteCreateCommand request,
            CancellationToken cancellationToken)
        {
            return await _noteService.Create(request);
        }
    }
}