using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Commands
{
    public class ConversationCreateCommand : BaseCommand, IRequest<Payload<ConversationCreateRequest>>
    {
        public ConversationCreateRequest ConversationCreateRequest { get; set; }
    }
}
