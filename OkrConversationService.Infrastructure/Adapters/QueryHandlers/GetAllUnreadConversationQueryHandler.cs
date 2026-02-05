using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class GetAllUnreadConversationQueryHandler : IRequestHandler<GetAllUnreadConversationQuery, Payload<UnreadConversationResponse>>
    {

        private readonly IConversationService _conversationService;
        public GetAllUnreadConversationQueryHandler(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }
        public async Task<Payload<UnreadConversationResponse>> Handle(GetAllUnreadConversationQuery request, CancellationToken cancellationToken)
        {
            return await _conversationService.GetAllUnreadConversation(request);
        }
    }
}
