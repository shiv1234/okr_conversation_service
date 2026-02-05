using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("FeedbackDetail")]
    public partial class FeedbackDetail
    {
        public FeedbackDetail()
        {
            Comments = new HashSet<Comment>();
            CriteriaFeedbackMappings = new HashSet<CriteriaFeedbackMapping>();
        }

        [Key]
        public long FeedbackDetailId { get; set; }
        public long FeedbackRequestId { get; set; }
        public int FeedbackOnTypeId { get; set; }
        public long FeedbackOnId { get; set; }
        [Column(TypeName = "text")]
        public string SharedRemark { get; set; }
        [Required]
        public bool? IsOneToOneRequested { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Required]
        public bool? IsActive { get; set; }
        public int? CriteriaTypeId { get; set; }

        [ForeignKey(nameof(FeedbackOnTypeId))]
        [InverseProperty(nameof(FeedbackOnTypeMaster.FeedbackDetails))]
        public virtual FeedbackOnTypeMaster FeedbackOnType { get; set; }
        [ForeignKey(nameof(FeedbackRequestId))]
        [InverseProperty("FeedbackDetails")]
        public virtual FeedbackRequest FeedbackRequest { get; set; }
        [InverseProperty(nameof(Comment.FeedbackDetail))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(CriteriaFeedbackMapping.FeedbackDetail))]
        public virtual ICollection<CriteriaFeedbackMapping> CriteriaFeedbackMappings { get; set; }
    }
}
