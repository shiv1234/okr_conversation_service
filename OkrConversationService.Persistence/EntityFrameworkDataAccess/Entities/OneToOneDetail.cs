using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("OneToOneDetail")]
    public partial class OneToOneDetail
    {
        [Key]
        public long OneToOneDetailId { get; set; }
        public int RequestType { get; set; }
        public long RequestId { get; set; }
        public long RequestedTo { get; set; }
        public long RequestedFrom { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string OneToOneRemark { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        public int? Status { get; set; }
        public string OneToOneTitle { get; set; }
        public bool IsLaunched { get; set; }
        public DateTime? LaunchCompletedDate { get; set; }
    }
}

