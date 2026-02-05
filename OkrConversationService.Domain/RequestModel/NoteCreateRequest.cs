using System.Collections.Generic;

namespace OkrConversationService.Domain.RequestModel
{
    public class NoteCreateRequest
    {
        public long NoteId { get; set; }
        public string Description { get; set; }
        public int GoalTypeId { get; set; }
        public long GoalId { get; set; }
        public List<NoteFiles> assignedFiles { get; set; } = new List<NoteFiles>();
        public List<NoteEmployeeTags> employeeTags { get; set; } = new List<NoteEmployeeTags>();
        public bool IsPrivate { get; set; } = false;
    }
}
