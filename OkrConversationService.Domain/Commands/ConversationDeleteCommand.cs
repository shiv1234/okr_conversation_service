using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Commands
{
    public class ConversationDeleteCommand : BaseCommand, IRequest<Payload<bool>>
    {
        public long ConversationId { get; set; }
    }
}
