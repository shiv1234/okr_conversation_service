using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class TeamGetAllQuery : IRequest<Payload<NoteResponse>>
    {
        public long GoalId { get; set; } = 1;
        public int GoalTypeId { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
