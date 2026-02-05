using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;


namespace OkrConversationService.Domain.Commands
{
    public class CommentCreateCommand : BaseCommand, IRequest<Payload<CommentDetailsRequest>>
    {
        public CommentDetailsRequest CommentDetailsRequest { get; set; }
    }
}
