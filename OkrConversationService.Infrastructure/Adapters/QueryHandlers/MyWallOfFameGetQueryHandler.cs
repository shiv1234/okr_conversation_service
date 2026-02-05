
using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class MyWallOfFameGetQueryHandler : IRequestHandler<MyWallOfFameGetQuery, Payload<MyWallOfFameResponse>>
    {
        private readonly IRecognitionService _recognitionService;
        public MyWallOfFameGetQueryHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<MyWallOfFameResponse>> Handle(MyWallOfFameGetQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.GetMyWallOfFameGetQuery(request);
        }
    }
}
