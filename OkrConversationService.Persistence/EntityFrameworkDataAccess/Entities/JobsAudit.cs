using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("JobsAudit")]
    public partial class JobsAudit
    {
        [Key]
        public long AuditId { get; set; }
        [Required]
        [StringLength(500)]
        public string JobName { get; set; }
        [StringLength(100)]
        public string Status { get; set; }
        [StringLength(500)]
        public string Details { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ExecutionDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AuditDate { get; set; }
    }
}
