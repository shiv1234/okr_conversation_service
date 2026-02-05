using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class CommentDeleteHandler : IRequestHandler<CommentDeleteCommand, Payload<bool>>
    {
        private readonly IRecognitionService _recognitionService;
        public CommentDeleteHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<bool>> Handle(CommentDeleteCommand request, CancellationToken cancellationToken)
        {
            return await _recognitionService.DeleteComment(request);
        }
    }
}
