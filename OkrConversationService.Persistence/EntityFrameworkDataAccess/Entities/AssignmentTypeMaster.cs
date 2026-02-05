using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("AssignmentTypeMaster")]
    public partial class AssignmentTypeMaster
    {
        [Key]
        public int AssignmentTypeId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        public bool? IsDefault { get; set; }
    }
}
