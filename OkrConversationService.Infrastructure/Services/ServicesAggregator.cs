using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;

namespace OkrConversationService.Infrastructure.Services
{
    public class ServicesAggregator : IServicesAggregator
    {
        public IUnitOfWorkAsync UnitOfWorkAsync { get; set; }
        public IOperationStatus OperationStatus { get; set; }
        public IConfiguration Configuration { get; set; }
        public IMapper Mapper { get; set; }
        public IWebHostEnvironment HostingEnvironment { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }

        public ServicesAggregator(IUnitOfWorkAsync unitOfWorkAsync, IOperationStatus operationStatus, IConfiguration configuration, IMapper mapper, IWebHostEnvironment environment, ILoggerFactory loggerFactory)
        {
            UnitOfWorkAsync = unitOfWorkAsync;
            OperationStatus = operationStatus;
            Configuration = configuration;
            HostingEnvironment = environment;
            Mapper = mapper;
            LoggerFactory = loggerFactory;
        }
    }
}
