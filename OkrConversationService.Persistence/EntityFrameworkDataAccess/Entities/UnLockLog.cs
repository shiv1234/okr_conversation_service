using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("UnLockLog")]
    public partial class UnLockLog
    {
        [Key]
        public long UnLockLogId { get; set; }
        public int? Year { get; set; }
        public int? Cycle { get; set; }
        public long? EmployeeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LockedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LockedTill { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? Status { get; set; }
        [Required]
        public bool? IsActive { get; set; }
    }
}
