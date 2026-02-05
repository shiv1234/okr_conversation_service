using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class CheckInWeeklyDatesQueryHandler : IRequestHandler<CheckInWeeklyDatesQuery, Payload<CheckInDatesPermissionResponse>>
    {
        private readonly ICheckInService _checkInService;
        public CheckInWeeklyDatesQueryHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }
        public async Task<Payload<CheckInDatesPermissionResponse>> Handle(CheckInWeeklyDatesQuery request, CancellationToken cancellationToken)
        {
            return await _checkInService.GetAllCheckInWeeklyDates(request);
        }
    }
}
