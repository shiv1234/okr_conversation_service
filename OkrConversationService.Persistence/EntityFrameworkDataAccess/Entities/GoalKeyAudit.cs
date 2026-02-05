using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("GoalKeyAudit")]
    [Index(nameof(UpdatedGoalKeyId), nameof(UpdatedColumn), Name = "NCI_GoalKeyAudit_UpdatedGoalKeyId_UpdatedColumn")]
    public partial class GoalKeyAudit
    {
        [Key]
        public long GoalKeyAuditId { get; set; }
        public long? UpdatedGoalKeyId { get; set; }
        [StringLength(250)]
        public string UpdatedColumn { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
