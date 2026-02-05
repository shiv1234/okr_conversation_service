using System;

namespace OkrConversationService.Domain.ResponseModels
{
    public class TaskResponse
    {
        public long TaskId { get; set; }
        public long GoalId { get; set; }
        public string KeyDescription { get; set; }
        public string TaskName { get; set; }
        public bool IsCompleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime TaskStartedDate { get; set; }
        public bool IsImported { get; set; }
    }
}
