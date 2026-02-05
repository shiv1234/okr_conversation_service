using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public partial class Email
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(60)]
        public string EmailAddress { get; set; }
        [StringLength(100)]
        public string FullName { get; set; }
    }
}
