using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class GetAllUnreadConversationQuery : IRequest<Payload<UnreadConversationResponse>>
    {
        public long EmpId { get; set; }
    }
}
