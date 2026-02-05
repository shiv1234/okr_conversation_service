using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [Keyless]
    [Table("TestTable")]

    public partial class TestTable
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string TestName { get; set; }
        [StringLength(10)]
        public string CountryStdCode { get; set; }
    }
}
