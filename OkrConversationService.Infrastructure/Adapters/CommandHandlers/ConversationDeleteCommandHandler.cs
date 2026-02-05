using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class ConversationDeleteCommandHandler : IRequestHandler<ConversationDeleteCommand, Payload<bool>>
    {
        private readonly IConversationService _conversationoteService;

        public ConversationDeleteCommandHandler(IConversationService conversationService)
        {
            _conversationoteService = conversationService;
        }

        public async Task<Payload<bool>> Handle(ConversationDeleteCommand request, CancellationToken cancellationToken)

        {
            return await _conversationoteService.DeleteConversation(request);
        }
    }
}
