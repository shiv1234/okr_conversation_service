using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("TeamSequence")]
    public partial class TeamSequence
    {
        [Key]
        public long TeamSequenceId { get; set; }
        public long TeamId { get; set; }
        public long EmployeeId { get; set; }
        public int CycleId { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}
