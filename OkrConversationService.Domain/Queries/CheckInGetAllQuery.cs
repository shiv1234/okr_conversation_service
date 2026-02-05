using MediatR;
using OkrConversationService.Domain.ResponseModels;
using System;

namespace OkrConversationService.Domain.Queries
{
    public class CheckInGetAllQuery : IRequest<Payload<CheckInPointsResponse>>
    {
        public long EmployeeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
