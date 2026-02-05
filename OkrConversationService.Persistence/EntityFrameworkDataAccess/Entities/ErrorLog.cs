using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("ErrorLog")]
    public partial class ErrorLog
    {
        [Key]
        public long LogId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Required]
        [StringLength(100)]
        public string PageName { get; set; }
        [Required]
        [StringLength(100)]
        public string FunctionName { get; set; }
        [Required]
        [StringLength(100)]
        public string ApplicationName { get; set; }
        [Required]
        public string ErrorDetail { get; set; }
    }
}
