using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class EmployeesLeaderBoardGetQuery : IRequest<Payload<RecognitionByTeamIdResponse>>
    {
        public EmployeesLeaderBoardRequest Request { get; set; }
    }
}
