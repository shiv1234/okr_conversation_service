using System.Collections.Generic;

namespace OkrConversationService.Domain.RequestModel
{
    public class NoteEditRequest
    {
        public long NoteId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<NoteFiles> assignedFiles { get; set; } = new List<NoteFiles>();
        public List<NoteEmployeeTags> employeeTags { get; set; } = new List<NoteEmployeeTags>();
        public bool IsPrivate { get; set; }
    }
}
