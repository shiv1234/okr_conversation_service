using System.Collections.Generic;

namespace OkrConversationService.Domain.RequestModel
{
    public class ConversationCreateRequest
    {
        public long ConversationId { get; set; }
        public string Description { get; set; }
        public int GoalTypeId { get; set; }
        public long GoalId { get; set; }
        public int Type { get; set; }
        public int GoalSourceId { get; set; }
        public List<ConversationFiles> assignedFiles { get; set; }
        public List<ConversationEmployeeTags> employeeTags { get; set; }
    }
}
