using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("ConfidenceAudit")]
    public partial class ConfidenceAudit
    {
        [Key]
        public long ConfidenceAuditId { get; set; }
        public long GoalKeyId { get; set; }
        public long ImportedId { get; set; }
        public long EmployeeId { get; set; }
        public int ConfidenceId { get; set; }
        public int ContributorAverage { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public int? CycleId { get; set; }
    }
}
