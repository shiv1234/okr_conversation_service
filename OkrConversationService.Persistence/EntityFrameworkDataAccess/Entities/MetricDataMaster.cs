using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Keyless]
    [Table("MetricDataMaster")]
    public partial class MetricDataMaster
    {
        public int DataId { get; set; }
        public int MetricId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        [StringLength(1)]
        public string Symbol { get; set; }
        public bool? IsDefault { get; set; }
    }
}
