using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Ports
{
    public class IsNoteEmployeeTagQuery : IRequest<Payload<bool>>
    {
        public long NoteId { get; set; }
    }
}
