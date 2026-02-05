using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("CriteriaFeedbackMapping")]
    public partial class CriteriaFeedbackMapping
    {
        [Key]
        public long CriteriaFeedbackMappingId { get; set; }
        public long? FeedbackDetailId { get; set; }
        public long? CriteriaMasterId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Score { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }

        [ForeignKey(nameof(CriteriaMasterId))]
        [InverseProperty("CriteriaFeedbackMappings")]
        public virtual CriteriaMaster CriteriaMaster { get; set; }
        [ForeignKey(nameof(FeedbackDetailId))]
        [InverseProperty("CriteriaFeedbackMappings")]
        public virtual FeedbackDetail FeedbackDetail { get; set; }
    }
}
