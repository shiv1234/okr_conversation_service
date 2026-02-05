using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("GoalKey")]
    [Index(nameof(CycleId), nameof(IsActive), nameof(EmployeeId), nameof(GoalStatusId), nameof(KrStatusId), nameof(ImportedId), nameof(ImportedType), nameof(TeamId), Name = "NCI_GoalKey_CycleId_IsActive_EmployeeId_GoalStatusId_KrStatusId_ImportedId_ImportedType_TeamId")]
    [Index(nameof(EmployeeId), nameof(CycleId), Name = "NCI_GoalKey_EmployeeId_CycleId")]
    [Index(nameof(KrStatusId), nameof(GoalObjectiveId), nameof(IsActive), Name = "NCI_GoalKey_KrStatusId_GoalObjectiveId_IsActive")]
    public partial class GoalKey
    {
        [Key]
        public long GoalKeyId { get; set; }
        public long GoalObjectiveId { get; set; }
        [Required]
        public string KeyDescription { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Score { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DueDate { get; set; }
        public int? ImportedType { get; set; }
        public long? ImportedId { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        public int? Progress { get; set; }
        public long Source { get; set; }
        public long EmployeeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        public int? MetricId { get; set; }
        public int? AssignmentTypeId { get; set; }
        public int? CurrencyId { get; set; }
        [Column(TypeName = "decimal(38, 2)")]
        public decimal? CurrentValue { get; set; }
        [Column(TypeName = "decimal(38, 2)")]
        public decimal? TargetValue { get; set; }
        public int? KrStatusId { get; set; }
        public int? CycleId { get; set; }
        [StringLength(10)]
        public string CurrencyCode { get; set; }
        public int? GoalStatusId { get; set; }
        [Column(TypeName = "decimal(38, 2)")]
        public decimal? ContributorValue { get; set; }
        [Column(TypeName = "decimal(38, 2)")]
        public decimal? StartValue { get; set; }
        [Column(TypeName = "text")]
        public string KeyNotes { get; set; }
        public long? TeamId { get; set; }
        public long? Owner { get; set; }
        public int ConfidenceId { get; set; }
        public int WeightId { get; set; }
        public bool IsScored { get; set; }
    }
}
