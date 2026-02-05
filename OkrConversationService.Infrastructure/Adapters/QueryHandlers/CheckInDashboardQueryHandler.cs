using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class CheckInDashboardQueryHandler : IRequestHandler<CheckInDashboardQuery, Payload<DashboardCheckInResponse>>
    {
        private readonly ICheckInService _checkInService;

        public CheckInDashboardQueryHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }

        public async Task<Payload<DashboardCheckInResponse>> Handle(CheckInDashboardQuery request,
            CancellationToken cancellationToken)
        {
            return await _checkInService.GetDashboardCheckInDetails(request);
        }
    }
}
