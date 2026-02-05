using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class CheckInGetAllQueryHandler : IRequestHandler<CheckInGetAllQuery, Payload<CheckInPointsResponse>>
    {
        private readonly ICheckInService _checkInService;
        public CheckInGetAllQueryHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }
        public async Task<Payload<CheckInPointsResponse>> Handle(CheckInGetAllQuery request, CancellationToken cancellationToken)
        {
            return await _checkInService.GetAll(request);
        }
    }
}
