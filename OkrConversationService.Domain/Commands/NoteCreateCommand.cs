using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Commands
{
    public class NoteCreateCommand : BaseCommand, IRequest<Payload<NoteCreateRequest>>
    {
        public NoteCreateRequest NoteCreateRequest { get; set; }
    }
}
