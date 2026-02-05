using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("GoalSequence")]
    public partial class GoalSequence
    {
        [Key]
        public long SequenceId { get; set; }
        public int GoalType { get; set; }
        public long GoalId { get; set; }
        public long EmployeeId { get; set; }
        public int GoalCycleId { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}
