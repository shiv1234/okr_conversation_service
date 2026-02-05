using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class CommentCreateCommandHandler : IRequestHandler<CommentCreateCommand, Payload<CommentDetailsRequest>>
    {
        private readonly IRecognitionService _recognitionService;
        public CommentCreateCommandHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<CommentDetailsRequest>> Handle(CommentCreateCommand request, CancellationToken cancellationToken)
        {
            return await _recognitionService.CreateComments(request);
        }
    }
}
