using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Domain.Validator;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace OkrConversationService.Application.Controllers
{
    [Route("conversation")]
    [ApiController]
    public class ConversationController : ApiControllerBase
    {
        private readonly ILogger<ConversationController> _logger;

        public ConversationController(ILoggerFactory loggerFactory, IMediator mediator, ICommonBase commonBase) : base(
            loggerFactory, mediator, commonBase)
        {
            _logger = LoggerFactory.CreateLogger<ConversationController>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(Payload<ConversationResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAll(long goalSourceId, int goalTypeId, int pageIndex = 1, int pageSize = 10)
        {
            _logger.LogInformation("ConversationController: GetAll Called! ");
            var payload = await Mediator.Send(new ConversationGetAllQuery() { GoalSourceId = goalSourceId, GoalTypeId = goalTypeId, PageIndex = pageIndex, PageSize = pageSize });
            return Ok(payload);
        }


        [HttpGet]
        [Route("ConversationReply")]
        [ProducesResponseType(typeof(Payload<ConversationCommentResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllConversationContents(long conversationId, int pageIndex = 1, int pageSize = 10)
        {
            _logger.LogInformation("ConversationController: GetAll Called! ");
            var payload = await Mediator.Send(new ConversationCommentGetAllQuery() { GoalId = conversationId,PageIndex = pageIndex, PageSize = pageSize });
            return Ok(payload);
        }


        [HttpPost]
        [Route("uploadConversation")]
        [ProducesResponseType(typeof(Payload<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UploadConversationImage([Required] IFormFile UploadFiles,int type)
        {
            _logger.LogInformation("ConversationController: UploadConversationImage Called! ");
            var command = new UploadFileCommand { FormFile = UploadFiles , Type = type };
            var payload = await Mediator.Send(command);
            return Ok(payload);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Payload<ConversationCreateRequest>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Create(ConversationCreateRequest request)
        {
            _logger.LogInformation("ConversationCreateRequestController: Create Called! ");
            var payload = new Payload<ConversationCreateRequest>();
            var command = new ConversationCreateCommand { ConversationCreateRequest = request };
            var validator = new ConversationCreateValidator();
            var state = await validator.ValidateAsync(request);

            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);

            return Ok(payload);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Payload<ConversationEditRequest>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Edit(ConversationEditRequest request)
        {
            _logger.LogInformation("ConversationController: Edit Called! ");
            var payload = new Payload<ConversationEditRequest>();
            var command = new ConversationEditCommand { ConversationEditRequest = request };
            var validator = new ConversationEditValidator();
            var state = await validator.ValidateAsync(request);

            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);

            return Ok(payload);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Payload<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([Required] long conversationId)
        {
            _logger.LogInformation("ConversationController: Delete Called! ");
            var payload = new Payload<bool>();
            var command = new ConversationDeleteCommand { ConversationId = conversationId };
            var validator = new DeleteConversationValidator();
            var state = await validator.ValidateAsync(command);
            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);

            return Ok(payload);
        }

        [HttpGet]
        [Route("unreadConversation")]
        [ProducesResponseType(typeof(Payload<UnreadConversationResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllUnreadConversation(long empId)
        {
            _logger.LogInformation("ConversationController: GetAll Called! ");
            var payload = await Mediator.Send(new GetAllUnreadConversationQuery() { EmpId = empId });
            return Ok(payload);
        }

        [HttpGet]
        [Route("IsEmployeeTag")]
        [ProducesResponseType(typeof(Payload<bool>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> IsEmployeeTag([Required] long conversationId)
        {
            _logger.LogInformation("ConversationController: IsEmployeeTag Called! ");
            var payload = await Mediator.Send(new IsEmployeeTagQuery() { ConversationId = conversationId });
            return Ok(payload);
        }

        [HttpPost]
        [Route("like")]
        [ProducesResponseType(typeof(Payload<ConversationLikeCreateRequest>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateLike(ConversationLikeCreateRequest request)
        {
            _logger.LogInformation("ConversationController: CreateLike Called! ");
            var payload = new Payload<ConversationLikeCreateRequest>();
            var command = new ConversationLikeCommand { ConversationLikeCreateRequest = request };
            var validator = new ConversationLikeValidator();
            var state = await validator.ValidateAsync(request);
            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);
            return Ok(payload);
        }
    }
}
