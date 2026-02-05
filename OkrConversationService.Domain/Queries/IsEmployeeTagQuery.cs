using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class IsEmployeeTagQuery : IRequest<Payload<bool>>
    {
        public long ConversationId { get; set; }
    }
}
