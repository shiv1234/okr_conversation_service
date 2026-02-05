using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Keyless]
    public partial class Gohierarchy
    {
        public long GoalObjectiveId { get; set; }
        public long EmployeeId { get; set; }
        public bool? IsPrivate { get; set; }
        public string ObjectiveName { get; set; }
        [Column(TypeName = "text")]
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
        public bool IsActive { get; set; }
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
