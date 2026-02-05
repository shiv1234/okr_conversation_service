using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{

    public class MyWallOfFameGetQuery : IRequest<Payload<MyWallOfFameResponse>>
    {
        public MyWallOfFameRequest MyMyWallOfFameRequest = new MyWallOfFameRequest { };
    }

}
