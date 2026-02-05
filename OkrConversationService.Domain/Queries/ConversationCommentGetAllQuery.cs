

using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class ConversationCommentGetAllQuery : IRequest<Payload<ConversationCommentResponse>>
    {
        public long GoalId { get; set; }       
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
