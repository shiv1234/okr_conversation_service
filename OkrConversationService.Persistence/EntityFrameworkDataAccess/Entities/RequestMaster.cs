using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("RequestMaster")]
    public partial class RequestMaster
    {
        [Key]
        public int RequestId { get; set; }
        [StringLength(250)]
        public string RequestName { get; set; }
        public bool? IsActive { get; set; }
    }
}
