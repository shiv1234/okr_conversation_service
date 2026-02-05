using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("EngagementType")]
    public partial class EngagementType
    {
        [Key]
        public int EngagementTypeId { get; set; }
        [Required]
        [StringLength(250)]
        public string EngagementTypeName { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }
    }
}
