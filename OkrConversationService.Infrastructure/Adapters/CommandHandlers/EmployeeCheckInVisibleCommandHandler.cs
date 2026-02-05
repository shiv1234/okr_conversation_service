using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class EmployeeCheckInVisibleCommandHandler : IRequestHandler<EmployeeCheckInVisibleCommand, Payload<bool>>
    {
        private readonly ICheckInService _checkInService;
        public EmployeeCheckInVisibleCommandHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }
        public async Task<Payload<bool>> Handle(EmployeeCheckInVisibleCommand request, CancellationToken cancellationToken)
        {
            return await _checkInService.AddEmployeeCheckInVisible(request);
        }
    }
}
