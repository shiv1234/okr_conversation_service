using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("CriteriaTypeMaster")]
    public partial class CriteriaTypeMaster
    {
        public CriteriaTypeMaster()
        {
            CriteriaMasters = new HashSet<CriteriaMaster>();
        }

        [Key]
        public int CriteriaTypeId { get; set; }
        [StringLength(50)]
        public string CriteriaTypeName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }

        [InverseProperty(nameof(CriteriaMaster.CriteriaType))]
        public virtual ICollection<CriteriaMaster> CriteriaMasters { get; set; }
    }
}
