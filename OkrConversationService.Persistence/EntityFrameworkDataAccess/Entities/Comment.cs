using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        public long CommentId { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Comments { get; set; }
        public long FeedbackDetailId { get; set; }
        public long ParentCommentId { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Required]
        public bool? IsActive { get; set; }

        [ForeignKey(nameof(FeedbackDetailId))]
        [InverseProperty("Comments")]
        public virtual FeedbackDetail FeedbackDetail { get; set; }
    }
}
