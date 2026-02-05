using System.Collections.Generic;

namespace OkrConversationService.Domain.RequestModel
{
    public class ConversationEditRequest
    {
        public long ConversationId { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public bool IsActive { get; set; }
        public List<ConversationFiles> assignedFiles { get; set; }
        public List<ConversationEmployeeTags> employeeTags { get; set; }
    }
}
