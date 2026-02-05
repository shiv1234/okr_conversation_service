using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [Keyless]
    [Table("TeamProgress_dummy")]

    public partial class TeamProgressDummy
    {
        public long TeamProgressId { get; set; }
        public long CycleId { get; set; }
        public long TeamId { get; set; }
        [Required]
        [StringLength(250)]
        public string TeamName { get; set; }
        public long TeamHead { get; set; }
        public int OkrCount { get; set; }
        public int KrCount { get; set; }
        public int MemberCount { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Score { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }
    }
}
