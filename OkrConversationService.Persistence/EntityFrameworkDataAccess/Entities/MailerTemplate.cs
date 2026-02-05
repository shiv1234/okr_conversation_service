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
    [Table("MailerTemplate")]
    public partial class MailerTemplate
    {
        public long MailerTemplateId { get; set; }
        [StringLength(250)]
        public string TemplateName { get; set; }
        [StringLength(20)]
        public string TemplateCode { get; set; }
        [StringLength(300)]
        public string Subject { get; set; }
        [Column(TypeName = "text")]
        public string Body { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
    }
}
