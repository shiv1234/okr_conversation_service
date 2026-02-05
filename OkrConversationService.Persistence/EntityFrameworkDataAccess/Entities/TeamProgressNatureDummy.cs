using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [Keyless]
    [Table("TeamProgressNature_dummy")]

    public partial class TeamProgressNatureDummy
    {
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
