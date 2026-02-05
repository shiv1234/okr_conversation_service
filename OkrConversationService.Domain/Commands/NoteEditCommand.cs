using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Commands
{
    public class NoteEditCommand : BaseCommand, IRequest<Payload<NoteEditRequest>>
    {
        public NoteEditRequest NoteEditRequest { get; set; }
    }
}
