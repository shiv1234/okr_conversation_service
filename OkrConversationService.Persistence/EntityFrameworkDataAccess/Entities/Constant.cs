using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("Constant")]
    public partial class Constant
    {
        [Key]
        public long ConstantId { get; set; }
        [StringLength(500)]
        public string ConstantName { get; set; }
        [StringLength(500)]
        public string ConstantValue { get; set; }
        [StringLength(500)]
        public string ConstantDescription { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
    }
}
