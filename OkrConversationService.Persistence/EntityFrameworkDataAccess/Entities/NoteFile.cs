using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public partial class NoteFile
    {
        [Key]
        public long NoteFileId { get; set; }
        public long NoteId { get; set; }
        [Required]
        [StringLength(100)]
        public string FileName { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public string StorageFileName { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}
