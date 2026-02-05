using System;

namespace OkrConversationService.Domain.ResponseModels
{
    public class UserNoteResponse
    {
        public long NoteId { get; set; }
        public string Description { get; set; }
        public int GoalTypeId { get; set; }
        public long GoalId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsEdited { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsPrivate { get; set; }
    }
}
