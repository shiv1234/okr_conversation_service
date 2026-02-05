namespace OkrConversationService.Persistence.EntityFrameworkDataAccess
{
    public interface IOkrAuthDbContext
    {
        string ConnectionString { get; }
        string Schema { get; }
    }
}
