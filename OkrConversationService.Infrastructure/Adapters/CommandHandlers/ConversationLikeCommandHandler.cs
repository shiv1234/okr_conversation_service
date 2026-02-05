using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class ConversationLikeCommandHandler : IRequestHandler<ConversationLikeCommand, Payload<ConversationLikeCreateRequest>>
    {
        private readonly IConversationService _conversationoteService;

        public ConversationLikeCommandHandler(IConversationService conversationService)
        {
            _conversationoteService = conversationService;
        }

        public async Task<Payload<ConversationLikeCreateRequest>> Handle(ConversationLikeCommand request,
            CancellationToken cancellationToken)
        {
            return await _conversationoteService.CreateLike(request);
        }
    }
}
