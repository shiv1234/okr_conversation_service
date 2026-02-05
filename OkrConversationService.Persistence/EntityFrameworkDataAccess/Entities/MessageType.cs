using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("MessageType")]
    public partial class MessageType
    {
        [Key]
        public long MessageTypeId { get; set; }
        [StringLength(250)]
        public string Message { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column("isdeleted")]
        public int? Isdeleted { get; set; }
    }
}
