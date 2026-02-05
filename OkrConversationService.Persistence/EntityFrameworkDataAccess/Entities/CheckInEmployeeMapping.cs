using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [Table("CheckInEmployeeMapping")]
    public partial class CheckInEmployeeMapping
    {
        [Key]
        public long CheckInEmployeeMappingId { get; set; }
        public long EmployeeId { get; set; }
        public int CheckInVisibilty { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
    }
}
