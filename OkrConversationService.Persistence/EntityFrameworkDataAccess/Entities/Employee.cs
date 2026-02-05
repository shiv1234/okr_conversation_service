using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Index(nameof(IsActive), nameof(ReportingTo), Name = "NCI_Employees_IsActive_ReportingTo")]
    [Index(nameof(EmailId), Name = "myindex", IsUnique = true)]
    public partial class Employee
    {
        public Employee()
        {
            EmployeeContactDetails = new HashSet<EmployeeContactDetail>();
        }

        [Key]
        public long EmployeeId { get; set; }
        [StringLength(30)]
        public string EmployeeCode { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Password { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string PasswordSalt { get; set; }
        [StringLength(200)]
        public string Designation { get; set; }
        [Required]
        [StringLength(100)]
        public string EmailId { get; set; }
        public long? ReportingTo { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [StringLength(500)]
        public string ProfileImageFile { get; set; }
        public int? LoginFailCount { get; set; }
        public bool IsSystemUser { get; set; }
        [InverseProperty(nameof(EmployeeContactDetail.Employee))]
        public virtual ICollection<EmployeeContactDetail> EmployeeContactDetails { get; set; }
    }
}
