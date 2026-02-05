using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Keyless]
    [Table("GuidedTour")]
    public partial class GuidedTour
    {
        public long Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
