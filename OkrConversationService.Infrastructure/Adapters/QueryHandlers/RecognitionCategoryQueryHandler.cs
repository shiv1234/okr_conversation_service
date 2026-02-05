using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class RecognitionCategoryQueryHandler : IRequestHandler<RecognitionCategoryGetQuery, Payload<RecognitionCategoryResponse>>
    {
        private readonly IRecognitionService _recognitionService;
        public RecognitionCategoryQueryHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<RecognitionCategoryResponse>> Handle(RecognitionCategoryGetQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.GetCategory(request);
        }
    }
}
