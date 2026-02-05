using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("EmployeeEngagement")]
    public partial class EmployeeEngagement
    {
        [Key]
        public long EmployeeEngagementId { get; set; }
        public long EmployeeId { get; set; }
        public long TeamId { get; set; }
        public long TeamHeadId { get; set; }
        public int EngagementTypeId { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }
    }
}
