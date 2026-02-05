using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public partial class TeamCycle
    {
        [Key]
        public long TeamCycleId { get; set; }
        public long TeamId { get; set; }
        public long CycleId { get; set; }
        public int GracePeriod { get; set; }
        public int PlanningPeriod { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CycleStartDate { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}
