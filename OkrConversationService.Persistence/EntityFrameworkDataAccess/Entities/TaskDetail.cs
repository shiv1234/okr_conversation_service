using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("TaskDetail")]
    public partial class TaskDetail
    {
        [Key]
        public long TaskId { get; set; }
        public int GoalTypeId { get; set; }
        public long GoalId { get; set; }
        [Required]
        public string TaskName { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CompletedDate { get; set; }
        public int TaskType { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime TaskStartedDate { get; set; }
        public bool IsImported { get; set; }
    }
}
