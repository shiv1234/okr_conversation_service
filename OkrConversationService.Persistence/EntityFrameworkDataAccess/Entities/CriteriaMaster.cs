using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("CriteriaMaster")]
    public partial class CriteriaMaster
    {
        public CriteriaMaster()
        {
            CriteriaFeedbackMappings = new HashSet<CriteriaFeedbackMapping>();
        }

        [Key]
        public long CriteriaMasterId { get; set; }
        public int? CriteriaTypeId { get; set; }
        [StringLength(30)]
        public string CriteriaName { get; set; }
        public long? OrganisationId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }

        [ForeignKey(nameof(CriteriaTypeId))]
        [InverseProperty(nameof(CriteriaTypeMaster.CriteriaMasters))]
        public virtual CriteriaTypeMaster CriteriaType { get; set; }
        [InverseProperty(nameof(CriteriaFeedbackMapping.CriteriaMaster))]
        public virtual ICollection<CriteriaFeedbackMapping> CriteriaFeedbackMappings { get; set; }
    }
}
