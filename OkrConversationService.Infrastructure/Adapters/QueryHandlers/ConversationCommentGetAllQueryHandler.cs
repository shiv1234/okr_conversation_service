using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class ConversationCommentGetAllQueryHandler : IRequestHandler<ConversationCommentGetAllQuery, Payload<ConversationCommentResponse>>
    {
        private readonly IConversationService _conversationService;
        public ConversationCommentGetAllQueryHandler(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }
        public async Task<Payload<ConversationCommentResponse>> Handle(ConversationCommentGetAllQuery request, CancellationToken cancellationToken)
        {
            return await _conversationService.GetConversationComments(request);
        }
    }
}
