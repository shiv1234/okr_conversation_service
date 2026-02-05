using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [Table("TeamProgressNature")]

    public partial class TeamProgressNature
    {
        [Key]
        public int TeamProgressNatureId { get; set; }
        public long TeamProgressId { get; set; }
        public long NatureTypeId { get; set; }
        public long OkrCount { get; set; }
        public long KrCount { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Score { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }
    }
}
