using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class TeamsByEmpIdGetQuery : IRequest<Payload<TeamByEmpIdResponse>>
    {
        public long EmployeeId { get; set; }
    }
}
