using System.Collections.Generic;
namespace OkrConversationService.Domain.RequestModel
{
    public class EmployeeCheckInVisibleReq
    {
        public long EmployeeId { get; set; }

    }
    public class EmployeeCheckInVisibleRequest
    {
        public List<EmployeeCheckInVisibleReq> EmployeeIds { get; set; } = new List<EmployeeCheckInVisibleReq>();
        public int CheckInVisibilty { get; set; } = 1;
    }

}
