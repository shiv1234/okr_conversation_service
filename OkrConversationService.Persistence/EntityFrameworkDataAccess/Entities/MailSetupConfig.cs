using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("MailSetupConfig")]
    public partial class MailSetupConfig
    {
        [Key]
        public long MailSetupConfigId { get; set; }
        [Required]
        [Column("AWSEmailID")]
        [StringLength(300)]
        public string AwsemailId { get; set; }
        [Required]
        [StringLength(250)]
        public string AccountName { get; set; }
        [StringLength(300)]
        public string AccountPassword { get; set; }
        [StringLength(250)]
        public string ServerName { get; set; }
        [Column("IsSSLEnable")]
        public bool IsSslenable { get; set; }
        public int Port { get; set; }
        [StringLength(300)]
        public string Host { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
    }
}
