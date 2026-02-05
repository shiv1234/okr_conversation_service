using MediatR;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OkrConversationService.Domain.Queries
{
    public class GetEmployeeCheckInVisibleQuery : IRequest<Payload<EmployeeCheckInVisibleResponse>>
    {
        public long EmpId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
