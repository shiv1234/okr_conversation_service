using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("RecognitionEmployeeTeamMapping")]
    public partial class RecognitionEmployeeTeamMapping
    {
        [Key]
        public long RecognitionEmployeeTeamMappingId { get; set; }
        public long RecognitionId { get; set; }
        public long EmployeeId { get; set; }
        public long? TeamId { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsGivenByManager { get; set; }
    }
}
