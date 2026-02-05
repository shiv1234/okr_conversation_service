using System;
namespace OkrConversationService.Domain.ResponseModels
{
    public class UnreadConversationResponse
    {
        public long ConversationId { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int GoalTypeId { get; set; }
        public long GoalId { get; set; }
        public long GoalSourceId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
