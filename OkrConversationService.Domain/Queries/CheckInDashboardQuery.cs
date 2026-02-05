using MediatR;
using OkrConversationService.Domain.ResponseModels;


namespace OkrConversationService.Domain.Queries
{
    public class CheckInDashboardQuery : IRequest<Payload<DashboardCheckInResponse>>
    {
        public long EmployeeId { get; set; }
    }
}
