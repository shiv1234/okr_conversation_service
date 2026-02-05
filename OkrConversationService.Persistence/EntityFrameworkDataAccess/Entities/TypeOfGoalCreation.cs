using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("TypeOfGoalCreation")]
    public partial class TypeOfGoalCreation
    {
        [Key]
        public long TypeOfGoalCreationId { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string PrimaryText { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string SecondaryText { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Required]
        public bool? IsActive { get; set; }
    }
}
