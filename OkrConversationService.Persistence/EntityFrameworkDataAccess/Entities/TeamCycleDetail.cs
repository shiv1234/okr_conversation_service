using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public partial class TeamCycleDetail
    {
        [Key]
        public long TeamCycleDetailId { get; set; }
        public long TeamId { get; set; }
        public long CycleId { get; set; }
        public int CycleSymbolId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CycleStartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CycleEndDate { get; set; }
        public int? CycleYear { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}
