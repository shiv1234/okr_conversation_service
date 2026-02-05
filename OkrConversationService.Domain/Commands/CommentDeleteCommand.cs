using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Commands
{
    public class CommentDeleteCommand : BaseCommand, IRequest<Payload<bool>>
    {
        public long CommentDetailsId { get; set; }
    }
}
