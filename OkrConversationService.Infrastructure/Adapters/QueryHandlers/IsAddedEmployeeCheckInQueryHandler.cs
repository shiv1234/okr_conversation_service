using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class IsAddedEmployeeCheckInQueryHandler : IRequestHandler<IsAddedEmployeeCheckInQuery,
         Payload<IsAddedEmployeeCheckInResponse>>
    {
        private readonly ICheckInService _checkInService;

        public IsAddedEmployeeCheckInQueryHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }

        public async Task<Payload<IsAddedEmployeeCheckInResponse>> Handle(IsAddedEmployeeCheckInQuery request,
            CancellationToken cancellationToken)
        {
            return await _checkInService.IsAddedEmployeeCheckIn(request);
        }
    }
}
