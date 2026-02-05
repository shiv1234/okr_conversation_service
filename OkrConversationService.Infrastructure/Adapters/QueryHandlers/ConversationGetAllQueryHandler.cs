using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class ConversationGetAllQueryHandler : IRequestHandler<ConversationGetAllQuery, Payload<ConversationResponse>>
    {

        private readonly IConversationService _conversationService;
        public ConversationGetAllQueryHandler(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }
        public async Task<Payload<ConversationResponse>> Handle(ConversationGetAllQuery request, CancellationToken cancellationToken)
        {
            return await _conversationService.GetAll(request);
        }
    }
}
