using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class ConversationGetAllQuery : IRequest<Payload<ConversationResponse>>
    {
        public long GoalSourceId { get; set; }
        public int GoalTypeId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
