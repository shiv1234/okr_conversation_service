using MediatR;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.Commands
{
    public class EmployeeCheckInVisibleCommand : BaseCommand, IRequest<Payload<bool>>
    {
        public EmployeeCheckInVisibleRequest EmployeeCheckInVisibleRequest { get; set; }
    }
}
