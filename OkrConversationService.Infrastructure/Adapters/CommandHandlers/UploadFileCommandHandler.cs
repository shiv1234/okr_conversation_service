using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, Payload<string>>
    {
        private readonly IConversationService _conversationService;

        public UploadFileCommandHandler(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        public async Task<Payload<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            return await _conversationService.UploadConversationImageOnAzure(request);
        }
    }
}
