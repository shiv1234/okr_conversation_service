using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("EmployeeContactDetail")]
    public partial class EmployeeContactDetail
    {
        [Key]
        public long ContactId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(25)]
        public string PhoneNumber { get; set; }
        [StringLength(25)]
        public string DeskPhoneNumber { get; set; }
        [StringLength(250)]
        public string SkypeUrl { get; set; }
        [StringLength(250)]
        public string TwitterUrl { get; set; }
        [StringLength(250)]
        public string LinkedInUrl { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [StringLength(10)]
        public string CountryStdCode { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty("EmployeeContactDetails")]
        public virtual Employee Employee { get; set; }
    }
}
