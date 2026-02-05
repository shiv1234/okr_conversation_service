using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class IsEmployeeTagQueryHandler : IRequestHandler<IsEmployeeTagQuery, Payload<bool>>
    {
        private readonly IConversationService _conversationService;
        public IsEmployeeTagQueryHandler(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }
        public async Task<Payload<bool>> Handle(IsEmployeeTagQuery request, CancellationToken cancellationToken)
        {
            return await _conversationService.IsEmployeeTag(request.ConversationId);
        }
    }
}

