using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("GoalObjective")]
    [Index(nameof(EmployeeId), nameof(ObjectiveCycleId), Name = "NCI_GoalObjective_EmployeeId_ObjectiveCycleId")]
    [Index(nameof(GoalStatusId), nameof(IsActive), nameof(ObjectiveCycleId), Name = "NCI_GoalObjective_GoalStatusId_IsActive_ObjectiveCycleId")]
    [Index(nameof(ImportedId), nameof(IsActive), nameof(LinkedObjectiveId), nameof(GoalStatusId), nameof(GoalTypeId), nameof(EmployeeId), nameof(Owner), nameof(TeamId), Name = "NCI_GoalObjective_ImportedId_IsActive_LinkedObjectiveId_GoalStatusId_GoalTypeId_EmployeeId_Owner_TeamId")]
    public partial class GoalObjective
    {
        [Key]
        public long GoalObjectiveId { get; set; }
        public long EmployeeId { get; set; }
        public bool? IsPrivate { get; set; }
        public string ObjectiveName { get; set; }
        public string ObjectiveDescription { get; set; }
        public int ObjectiveCycleId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        public int? ImportedType { get; set; }
        public long? ImportedId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Score { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        public int? Year { get; set; }
        public int? Progress { get; set; }
        public long Source { get; set; }
        public int? Sequence { get; set; }
        public int? GoalStatusId { get; set; }
        public int? GoalTypeId { get; set; }
        public long? TeamId { get; set; }
        public long? Owner { get; set; }
        public long? LinkedObjectiveId { get; set; }
        public bool IsCoachCreation { get; set; }
        public int? GoalNatureId { get; set; }
    }
}
