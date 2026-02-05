using OkrConversationService.Persistence.EntityFrameworkDataAccess;
namespace OkrConversationService.Infrastructure.Services.Contracts
{
    public interface IBaseService
    {
        IUnitOfWorkAsync UnitOfWorkAsync { get; set; }
        IOperationStatus OperationStatus { get; set; }
        OkrAuthDbContext OkrConversationDbContext { get; set; }        
    }
}
