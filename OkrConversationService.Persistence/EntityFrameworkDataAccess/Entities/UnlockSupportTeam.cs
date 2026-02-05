using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("UnlockSupportTeam")]
    public partial class UnlockSupportTeam
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(80)]
        public string EmailId { get; set; }
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
    }
}
