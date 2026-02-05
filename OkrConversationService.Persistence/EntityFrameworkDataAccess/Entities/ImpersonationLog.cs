using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class ImpersonationLog
    {
        [Key]
        public long ImpersonationLogId { get; set; }
        public long ImpersonatedById { get; set; }
        [StringLength(100)]
        public string ImpersonatedByUserName { get; set; }
        public long ImpersonatedUserId { get; set; }
        [StringLength(100)]
        public string ImpersonatedUserName { get; set; }       
        public int ActivityId { get; set; }        
        public string ActivityDescription { get; set; }
        public long TransactionId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ActivityDate { get; set; }
        
    }
}
