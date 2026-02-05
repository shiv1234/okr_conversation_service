using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("OnBoardingControl")]
    public partial class OnBoardingControl
    {
        [Key]
        public int Id { get; set; }
        [Column("employeeId")]
        public long? EmployeeId { get; set; }
        [Column("skipCount")]
        public int? SkipCount { get; set; }
        public int? ReadyCount { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column("updatedOn", TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
    }
}
