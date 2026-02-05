using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class TeamsLeaderBoardGetQuery : IRequest<Payload<RecognitionTeamsResponse>>
    {
        public RecognitionLeaderBoardRequest Request { get; set; }
    }
}
