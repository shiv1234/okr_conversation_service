using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Index(nameof(ApplicationMasterId), nameof(MessageTypeId), nameof(NotificationsTo), nameof(IsDeleted), Name = "NCI_NotificationsDetails_ApplicationMasterId_MessageTypeId_NotificationsTo_IsDeleted")]
    public partial class NotificationsDetail
    {
        [Key]
        public long NotificationsDetailsId { get; set; }
        public long? NotificationsBy { get; set; }
        public long? NotificationsTo { get; set; }
        [Column(TypeName = "text")]
        public string NotificationsMessage { get; set; }
        public int? ApplicationMasterId { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsDeleted { get; set; }
        public long? NotificationTypeId { get; set; }
        public long? MessageTypeId { get; set; }
        [Column(TypeName = "text")]
        public string Url { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? NotificationOnTypeId { get; set; }
        public long? NotificationOnId { get; set; }
        public bool Actionable { get; set; }
    }
}
