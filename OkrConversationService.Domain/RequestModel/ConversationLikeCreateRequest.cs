namespace OkrConversationService.Domain.RequestModel
{
    public class ConversationLikeCreateRequest
    {
        public long ModuleDetailsId { get; set; }
        public int ModuleId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsActive { get; set; }
    }
}
