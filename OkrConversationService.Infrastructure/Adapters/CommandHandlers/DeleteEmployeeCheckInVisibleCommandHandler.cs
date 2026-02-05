using MediatR;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.CommandHandlers
{
    public class DeleteEmployeeCheckInVisibleCommandHandler : IRequestHandler<DeleteEmployeeCheckInVisibleCommand, Payload<bool>>
    {
        private readonly ICheckInService _checkInService;
        public DeleteEmployeeCheckInVisibleCommandHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }
        public async Task<Payload<bool>> Handle(DeleteEmployeeCheckInVisibleCommand request, CancellationToken cancellationToken)
        {
            return await _checkInService.DeleteEmployeeCheckinVisible(request);
        }
    }
}
