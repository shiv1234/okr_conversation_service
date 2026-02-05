using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("ConfidenceMaster")]
    public partial class ConfidenceMaster
    {
        [Key]
        public int ConfidenceId { get; set; }
        public int? ConfidenceValue { get; set; }
        [StringLength(50)]
        public string ConfidenceName { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDefault { get; set; }
    }
}
