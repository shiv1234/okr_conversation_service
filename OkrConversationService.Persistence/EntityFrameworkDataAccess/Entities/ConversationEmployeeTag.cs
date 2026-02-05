using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{

    public partial class ConversationEmployeeTag
    {
        [Key]
        public long ConversationEmployeeTagId { get; set; }
        public long ConversationId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}
