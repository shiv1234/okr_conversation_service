using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public partial class Recognition
    {
        [Key]
        public long RecognitionId { get; set; }
        public long ReceiverId { get; set; } = 0;
        [Required]
        [StringLength(400)]
        public string Headlines { get; set; } = "";
        [Required]
        public string Message { get; set; }
        public bool IsAttachment { get; set; }
        public bool IsGivenByManager { get; set; }
        public int RecognitionCategoryTypeId { get; set; }
        public bool IsActive { get; set; }
      
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}
