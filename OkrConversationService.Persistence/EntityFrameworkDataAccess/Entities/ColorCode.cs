using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public partial class ColorCode
    {
        [Key]
        public int ColorCodeId { get; set; }
        [Column("ColorCode")]
        [StringLength(100)]
        public string ColorCode1 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [StringLength(100)]
        public string BackGroundColorCode { get; set; }
    }
}
