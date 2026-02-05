using System.Diagnostics.CodeAnalysis;

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess
{
    [ExcludeFromCodeCoverage]
    public class OkrAuthDbContext : IOkrAuthDbContext
    {
        public string Schema { get; }

        public string ConnectionString { get; }

        public OkrAuthDbContext(string connectionString, string schema)
        {
            ConnectionString = connectionString;
            Schema = schema;
        }
    }
}
