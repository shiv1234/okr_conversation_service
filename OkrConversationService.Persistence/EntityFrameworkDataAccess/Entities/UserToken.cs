using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("UserToken")]
    public partial class UserToken
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        [Required]
        public string Token { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ExpireTime { get; set; }
        public int? TokenType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastLoginDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CurrentLoginDate { get; set; }
    }
}
