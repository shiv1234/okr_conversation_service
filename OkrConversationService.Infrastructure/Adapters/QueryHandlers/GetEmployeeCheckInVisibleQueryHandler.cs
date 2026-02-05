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
    public class GetEmployeeCheckInVisibleQueryHandler : IRequestHandler<GetEmployeeCheckInVisibleQuery,
        Payload<EmployeeCheckInVisibleResponse>>
    {
        private readonly ICheckInService _checkInService;

        public GetEmployeeCheckInVisibleQueryHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }

        public async Task<Payload<EmployeeCheckInVisibleResponse>> Handle(GetEmployeeCheckInVisibleQuery request,
            CancellationToken cancellationToken)
        {
            return await _checkInService.GetEmployeeCheckInVisible(request);
        }
    }
}

