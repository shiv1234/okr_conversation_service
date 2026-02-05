using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public  class UserNoteAllQuery : IRequest<Payload<UserNoteResponse>>
    {
        public long GoalId { get; set; }
        public int GoalTypeId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
