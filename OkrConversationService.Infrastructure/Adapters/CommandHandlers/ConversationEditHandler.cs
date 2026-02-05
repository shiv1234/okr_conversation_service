using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;
namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class ConversationEditHandler : IRequestHandler<ConversationEditCommand, Payload<ConversationEditRequest>>
    {
        private readonly IConversationService _conversationoteService;

        public ConversationEditHandler(IConversationService conversationService)
        {
            _conversationoteService = conversationService;
        }

        public async Task<Payload<ConversationEditRequest>> Handle(ConversationEditCommand request,
            CancellationToken cancellationToken)
        {
            return await _conversationoteService.Edit(request);
        }
    }
}