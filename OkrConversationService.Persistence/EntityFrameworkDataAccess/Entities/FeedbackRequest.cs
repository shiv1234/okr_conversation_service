using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("FeedbackRequest")]
    public partial class FeedbackRequest
    {
        public FeedbackRequest()
        {
            FeedbackDetails = new HashSet<FeedbackDetail>();
        }

        [Key]
        public long FeedbackRequestId { get; set; }
        public int RaisedTypeId { get; set; }
        public long RaisedForId { get; set; }
        public long FeedbackById { get; set; }
        public int FeedbackOnTypeId { get; set; }
        public long FeedbackOnId { get; set; }
        [Column(TypeName = "text")]
        public string RequestRemark { get; set; }
        public int? FeedbackRequestType { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int Status { get; set; }
        [Required]
        public bool? IsActive { get; set; }

        [ForeignKey(nameof(FeedbackOnTypeId))]
        [InverseProperty(nameof(FeedbackOnTypeMaster.FeedbackRequests))]
        public virtual FeedbackOnTypeMaster FeedbackOnType { get; set; }
        [ForeignKey(nameof(RaisedTypeId))]
        [InverseProperty(nameof(RaisedTypeMaster.FeedbackRequests))]
        public virtual RaisedTypeMaster RaisedType { get; set; }
        [ForeignKey(nameof(Status))]
        [InverseProperty(nameof(StatusMaster.FeedbackRequests))]
        public virtual StatusMaster StatusNavigation { get; set; }
        [InverseProperty(nameof(FeedbackDetail.FeedbackRequest))]
        public virtual ICollection<FeedbackDetail> FeedbackDetails { get; set; }
    }
}
