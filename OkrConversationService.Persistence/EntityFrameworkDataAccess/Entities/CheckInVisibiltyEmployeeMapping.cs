using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [Table("EmployeeCheckInVisiblePermissions")]
    public partial  class EmployeeCheckInVisiblePermissions
    {
        [Key]
        public long EmployeeCheckInVisiblePermissionId { get; set; }
        public long EmployeeId { get; set; }

        public bool IsActive { get; set; }
        
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
