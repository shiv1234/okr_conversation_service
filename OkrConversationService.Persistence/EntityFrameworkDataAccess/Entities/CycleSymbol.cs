using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public partial class CycleSymbol
    {
        [Key]
        public int CycleSymbolId { get; set; }
        public long CycleId { get; set; }
        [Required]
        [StringLength(20)]
        public string SymbolName { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
