using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class TotalRecognitionByTeamIdGetQuery : IRequest<Payload<TotalRecognitionByTeamIdResponse>>
    {
        public TotalRecognitionByTeamIdRequest Team { get; set; }
    }
}
