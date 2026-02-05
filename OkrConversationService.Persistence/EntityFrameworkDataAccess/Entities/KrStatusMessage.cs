using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("KrStatusMessage")]
    public partial class KrStatusMessage
    {
        [Key]
        public int KrStatusMessageId { get; set; }
        public long AssignerGoalKeyId { get; set; }
        public long AssigneeGoalKeyId { get; set; }
        [Column(TypeName = "text")]
        public string KrAssignerMessage { get; set; }
        [Column(TypeName = "text")]
        public string KrAssigneeMessage { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOnAssigner { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOnAssignee { get; set; }
        public bool? IsActive { get; set; }
    }
}
