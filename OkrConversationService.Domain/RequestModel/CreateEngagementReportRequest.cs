
namespace OkrConversationService.Domain.RequestModel
{
    public class CreateEngagementReportRequest
    {
        public long EmployeeId { get; set; }
        public int EngagementTypeId { get; set; }
    }
}
