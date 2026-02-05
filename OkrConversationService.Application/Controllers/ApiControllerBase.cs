using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkrConversationService.Domain.Ports;

namespace OkrConversationService.Application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        public ILoggerFactory LoggerFactory { get; set; }
        public IMediator Mediator { get; set; }
#pragma warning disable S1104 // Fields should not have public accessibility
        public ICommonBase CommonBase;
#pragma warning restore S1104 // Fields should not have public accessibility
        public ApiControllerBase(ILoggerFactory loggerFactory, IMediator mediator, ICommonBase commonBase)
        {
            LoggerFactory = loggerFactory;
            Mediator = mediator;
            CommonBase = commonBase;
        }
    }
}
