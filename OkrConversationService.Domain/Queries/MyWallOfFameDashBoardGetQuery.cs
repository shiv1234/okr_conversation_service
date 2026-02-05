using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class MyWallOfFameDashBoardGetQuery : IRequest<Payload<MyWallOfFameDashBoardResponse>>
    {
    }
}
