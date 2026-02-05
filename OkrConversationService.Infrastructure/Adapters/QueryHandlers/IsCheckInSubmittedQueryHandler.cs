using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class IsCheckInSubmittedQueryHandler : IRequestHandler<IsCheckInSubmittedQuery, Payload<CheckInAlertResponse>>
    {
        private readonly ICheckInService _checkInService;
        public IsCheckInSubmittedQueryHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }
        public async Task<Payload<CheckInAlertResponse>> Handle(IsCheckInSubmittedQuery request, CancellationToken cancellationToken)
        {
            return await _checkInService.IsCheckInSubmitted();
        }
    }
}
