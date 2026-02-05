using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("OnBoardingScreen")]
    public partial class OnBoardingScreen
    {
        [Key]
        public int ScreenId { get; set; }
        public int? PageId { get; set; }
        [StringLength(100)]
        public string PageName { get; set; }
        [StringLength(50)]
        public string ControlType { get; set; }
        [Column(TypeName = "text")]
        public string ControlValue { get; set; }
        public int? Flow { get; set; }
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
