using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("NotificationType")]
    public partial class NotificationType
    {
        [Key]
        public long NotificationTypeId { get; set; }
        [StringLength(250)]
        public string Notification { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column("isdeleted")]
        public int? Isdeleted { get; set; }
        [Required]
        [StringLength(50)]
        public string NotificationCode { get; set; }
    }
}
