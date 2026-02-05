using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Commands
{
    public class ConversationLikeCommand : BaseCommand, IRequest<Payload<ConversationLikeCreateRequest>>
    {
        public ConversationLikeCreateRequest ConversationLikeCreateRequest { get; set; }
    }
}
