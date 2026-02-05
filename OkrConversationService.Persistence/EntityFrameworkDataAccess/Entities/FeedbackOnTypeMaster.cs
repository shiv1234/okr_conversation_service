using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("FeedbackOnTypeMaster")]
    public partial class FeedbackOnTypeMaster
    {
        public FeedbackOnTypeMaster()
        {
            FeedbackDetails = new HashSet<FeedbackDetail>();
            FeedbackRequests = new HashSet<FeedbackRequest>();
        }

        [Key]
        public int FeedbackOnTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public bool? IsActive { get; set; }

        [InverseProperty(nameof(FeedbackDetail.FeedbackOnType))]
        public virtual ICollection<FeedbackDetail> FeedbackDetails { get; set; }
        [InverseProperty(nameof(FeedbackRequest.FeedbackOnType))]
        public virtual ICollection<FeedbackRequest> FeedbackRequests { get; set; }
    }
}
