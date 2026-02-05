using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("RaisedTypeMaster")]
    public partial class RaisedTypeMaster
    {
        public RaisedTypeMaster()
        {
            FeedbackRequests = new HashSet<FeedbackRequest>();
        }

        [Key]
        public int RaisedTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public bool? IsActive { get; set; }

        [InverseProperty(nameof(FeedbackRequest.RaisedType))]
        public virtual ICollection<FeedbackRequest> FeedbackRequests { get; set; }
    }
}
