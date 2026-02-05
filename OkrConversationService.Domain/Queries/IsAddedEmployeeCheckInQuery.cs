using MediatR;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.Queries
{
   public class IsAddedEmployeeCheckInQuery : IRequest<Payload<IsAddedEmployeeCheckInResponse>>
    {
        public long EmployeeId { get; set; }
    }
}
