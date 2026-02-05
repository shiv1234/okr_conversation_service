using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Keyless]
    [Table("EmployeeOkrProgress_dummy")]
    public partial class EmployeeOkrProgressDummy
    {
        public long EmployeeOkrProgressId { get; set; }
        public long CycleId { get; set; }
        public long EmployeeId { get; set; }
        public long TeamId { get; set; }
        public long TeamHeadId { get; set; }
        public long OkrCount { get; set; }
        public long KrCount { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal IndivisualOkrScore { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TeamOkrScore { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Score { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }
    }
}
