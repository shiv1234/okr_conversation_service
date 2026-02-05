using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;
namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{

    public class MyWallOfFameDashBoardGetQueryHandler : IRequestHandler<MyWallOfFameDashBoardGetQuery, Payload<MyWallOfFameDashBoardResponse>>
    {
        private readonly IRecognitionService _recognitionService;
        public MyWallOfFameDashBoardGetQueryHandler(IRecognitionService recognitionService)
        {
            _recognitionService = recognitionService;
        }
        public async Task<Payload<MyWallOfFameDashBoardResponse>> Handle(MyWallOfFameDashBoardGetQuery request, CancellationToken cancellationToken)
        {
            return await _recognitionService.MyWallOfFameDashBoard(request);
        }
    }
}
