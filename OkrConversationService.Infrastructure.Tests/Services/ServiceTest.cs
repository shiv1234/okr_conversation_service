using Moq;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;

namespace OkrConversationService.Infrastructure.Tests.Services
{
    public class ServiceTest
    {
        public ServiceTest()
        {
            var checkInPointRepo = new Mock<IRepositoryAsync<CheckInPoint>>();
            var checkInDetailRepo = new Mock<IRepositoryAsync<CheckInDetail>>();
            var crossEmployeeRepo = new Mock<IRepositoryAsync<Employee>>();
            var servicesAggregator = new Mock<IServicesAggregator>();
            var unitOfWork = new Mock<IUnitOfWorkAsync>();

            unitOfWork.Setup(x => x.RepositoryAsync<CheckInPoint>()).Returns(checkInPointRepo.Object);
            unitOfWork.Setup(x => x.RepositoryAsync<CheckInDetail>()).Returns(checkInDetailRepo.Object);
            unitOfWork.Setup(x => x.RepositoryAsync<Employee>()).Returns(crossEmployeeRepo.Object);


            servicesAggregator.Setup(x => x.UnitOfWorkAsync).Returns(unitOfWork.Object);
        }
    }
}
