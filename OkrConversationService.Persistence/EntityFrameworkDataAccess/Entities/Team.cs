using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public partial class Team
    {
        [Key]
        public long TeamId { get; set; }
        [Required]
        [StringLength(250)]
        public string TeamName { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public long? TeamHead { get; set; }
        public long? ParentId { get; set; }
        [StringLength(100)]
        public string LogoName { get; set; }
        public string LogoImagePath { get; set; }
        [StringLength(100)]
        public string Colorcode { get; set; }
        [StringLength(100)]
        public string BackGroundColorCode { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}
