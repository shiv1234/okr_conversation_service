using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Keyless]
    [Table("EmployeeProgressNature_dummy")]
    public partial class EmployeeProgressNatureDummy
    {
        public long EmployeeProgressNatureId { get; set; }
        public long EmployeeOkrProgressId { get; set; }
        public long NatureTypeId { get; set; }
        public long OkrCount { get; set; }
        public long KrCount { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Score { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}
