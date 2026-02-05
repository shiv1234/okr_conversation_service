using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public partial class TeamSetting
    {
        [Key]
        public long TeamSettingId { get; set; }
        public long TeamId { get; set; }
        public int CadenceDays { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? EmailPlanningPeriod { get; set; }
        public bool? IsEmailPlanningPeriod { get; set; }
        public int? EmailExecutionPeriod { get; set; }
        public bool? IsEmailExecutionPeriod { get; set; }
        public int CheckInVisibilty { get; set; }
        public bool IsChangeCheckInVisibilty { get; set; }
    }
}
