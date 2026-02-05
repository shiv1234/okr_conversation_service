using MediatR;
using OkrConversationService.Domain.ResponseModels;

namespace OkrConversationService.Domain.Queries
{
    public class CheckInWeeklyDatesQuery : IRequest<Payload<CheckInDatesPermissionResponse>>
    {
        public long EmployeeId { get; set; }
    }
}
