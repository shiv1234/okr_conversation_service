using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class ConversationCreateHandler : IRequestHandler<ConversationCreateCommand, Payload<ConversationCreateRequest>>
    {
        private readonly IConversationService _conversationoteService;

        public ConversationCreateHandler(IConversationService conversationService)
        {
            _conversationoteService = conversationService;
        }

        public async Task<Payload<ConversationCreateRequest>> Handle(ConversationCreateCommand request,
            CancellationToken cancellationToken)
        {
            return await _conversationoteService.Create(request);
        }
    }
}