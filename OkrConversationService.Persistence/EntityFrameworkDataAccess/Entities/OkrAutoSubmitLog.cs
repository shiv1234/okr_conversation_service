using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Keyless]
    [Table("okrAutoSubmitLog")]
    public partial class OkrAutoSubmitLog
    {
        public long Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastTransaction { get; set; }
    }
}
