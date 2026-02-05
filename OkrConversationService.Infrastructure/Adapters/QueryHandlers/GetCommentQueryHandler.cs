using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, Payload<CommentResponse>>
    {
        private readonly IRecognitionService _recognitionService;
        public GetCommentQueryHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<CommentResponse>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.GetComments(request);
        }
    }
}
