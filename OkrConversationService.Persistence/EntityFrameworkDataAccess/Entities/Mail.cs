using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Keyless]
    public partial class Mail
    {
        public long MailId { get; set; }
        [StringLength(400)]
        public string MailTo { get; set; }
        [StringLength(100)]
        public string MailFrom { get; set; }
        [Column("CC")]
        [StringLength(500)]
        public string Cc { get; set; }
        [Column("BCC")]
        [StringLength(100)]
        public string Bcc { get; set; }
        [StringLength(400)]
        public string Subject { get; set; }
        [Column(TypeName = "text")]
        public string Body { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
    }
}
