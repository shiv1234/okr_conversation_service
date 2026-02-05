using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Keyless]
    [Table("OkrTypeFilter")]
    public partial class OkrTypeFilter
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string StatusName { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Color { get; set; }
        public bool? IsActive { get; set; }
    }
}
