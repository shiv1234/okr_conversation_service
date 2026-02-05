using System;

namespace OkrConversationService.Domain.RequestModel
{
    public class CheckInDetailRequest
    {
        public long CheckInDetailsId { get; set; }
        public int CheckInPointsId { get; set; }
        public string CheckInDetails { get; set; }
        public long EmployeeId { get; set; }
        public DateTime CheckInDate { get; set; } = DateTime.UtcNow;
    }
}
