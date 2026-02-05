using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("ProgressAudit")]
    public partial class ProgressAudit
    {
        [Key]
        public long ProgressAuditId { get; set; }
        public long GoalKeyId { get; set; }
        public long ImportedId { get; set; }
        public long EmployeeId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Score { get; set; }
        [Column(TypeName = "decimal(38, 2)")]
        public decimal? CurrentValue { get; set; }
        [Column(TypeName = "decimal(38, 2)")]
        public decimal? ContributorValue { get; set; }
        public int ContributorAverage { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public int? CycleId { get; set; }
    }
}
