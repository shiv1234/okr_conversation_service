namespace OkrConversationService.Domain.RequestModel
{
    public class AuditLogRequest
    {
        public string ActivityName { get; set; }
        public long TransactionId { get; set; }
        public string ActivityDescription { get; set; }
        public string ActivityPreviousData { get; set; }
    }
}
