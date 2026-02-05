using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("ApplicationMaster")]
    public partial class ApplicationMaster
    {
        [Key]
        public int ApplicationMasterId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        [StringLength(250)]
        public string AppName { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
