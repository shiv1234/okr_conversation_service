using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("GoalKeyHistory")]
    [Index(nameof(GoalKeyId), nameof(CreatedBy), nameof(CreatedOn), Name = "NCI_GoalKeyHistory_GoalKeyId_CreatedBy_CreatedOn")]
    public partial class GoalKeyHistory
    {
        [Key]
        public long HistoryId { get; set; }
        public long GoalKeyId { get; set; }
        [Column(TypeName = "decimal(38, 2)")]
        public decimal? CurrentValue { get; set; }
        [Column(TypeName = "decimal(38, 2)")]
        public decimal? ContributorValue { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Score { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public int? Progress { get; set; }
    }
}
