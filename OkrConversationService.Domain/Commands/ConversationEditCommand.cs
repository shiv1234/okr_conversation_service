using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
namespace OkrConversationService.Domain.Commands
{
    public class ConversationEditCommand : BaseCommand, IRequest<Payload<ConversationEditRequest>>
    {
        public ConversationEditRequest ConversationEditRequest { get; set; }
    }
}

