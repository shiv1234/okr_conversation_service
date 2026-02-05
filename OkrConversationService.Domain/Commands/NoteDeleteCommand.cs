using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Commands
{
    public class NoteDeleteCommand : BaseCommand, IRequest<Payload<long>>
    {
        public long NoteId { get; set; }
    }
}
