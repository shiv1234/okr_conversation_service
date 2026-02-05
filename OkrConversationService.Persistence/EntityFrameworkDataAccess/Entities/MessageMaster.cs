using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("MessageMaster")]
    public partial class MessageMaster
    {
        [Key]
        public int MessageMasterId { get; set; }
        [StringLength(1000)]
        public string MessageDesc { get; set; }
        public bool? IsActive { get; set; }
    }
}
