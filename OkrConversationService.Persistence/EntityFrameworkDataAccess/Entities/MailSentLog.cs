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
    [Table("MailSentLog")]
    public partial class MailSentLog
    {
        public long MailSentLogId { get; set; }
        [StringLength(250)]
        public string MailTo { get; set; }
        [StringLength(250)]
        public string MailFrom { get; set; }
        [Column("CC")]
        [StringLength(250)]
        public string Cc { get; set; }
        [Column("BCC")]
        [StringLength(250)]
        public string Bcc { get; set; }
        [StringLength(250)]
        public string MailSubject { get; set; }
        [Column(TypeName = "text")]
        public string Body { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? MailSentOn { get; set; }
        public bool? IsMailSent { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(20)]
        public string TemplateCode { get; set; }
    }
}
