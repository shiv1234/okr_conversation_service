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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace OkrConversationService.Application.Controllers
{
    [Route("notes")]
    [ApiController]
    public class NoteController : ApiControllerBase
    {
        private readonly ILogger<NoteController> _logger;

        public NoteController(ILoggerFactory loggerFactory, IMediator mediator, ICommonBase commonBase) : base(
            loggerFactory, mediator, commonBase)
        {
            _logger = LoggerFactory.CreateLogger<NoteController>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(Payload<NoteResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAll(long goalId, int goalTypeId, int pageIndex = 1, int pageSize = 10)
        {
            _logger.LogInformation("NoteController: GetAll Called! ");
            var payload = await Mediator.Send(new TeamGetAllQuery() { GoalId = goalId, GoalTypeId = goalTypeId, PageIndex = pageIndex, PageSize = pageSize });
            return Ok(payload);
        }

        [HttpPost]
        [Route("UploadNotes")]
        [ProducesResponseType(typeof(Payload<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UploadNotesImage([Required] IFormFile UploadFiles)
        {
            _logger.LogInformation("NoteController: UploadNotesImage Called! ");
            var command = new UploadFileCommand { FormFile = UploadFiles };
            var payload = await Mediator.Send(command);
            return Ok(payload);
        }


        [HttpDelete]
        [ProducesResponseType(typeof(Payload<long>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([Required] long noteId)
        {
            _logger.LogInformation("NoteController: Delete Called! ");
            var payload = new Payload<long>();
            var command = new NoteDeleteCommand { NoteId = noteId };
            var validator = new DeleteNoteValidator();
            var state = await validator.ValidateAsync(command);

            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);

            return Ok(payload);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Payload<NoteCreateRequest>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Create(NoteCreateRequest request)
        {
            _logger.LogInformation("NoteController: Create Called! ");
            var payload = new Payload<NoteCreateRequest>();
            var command = new NoteCreateCommand { NoteCreateRequest = request };
            var validator = new NoteCreateValidator();
            var state = await validator.ValidateAsync(request);

            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);

            return Ok(payload);
        }
        [HttpPut]
        [ProducesResponseType(typeof(Payload<NoteEditRequest>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Edit(NoteEditRequest request)
        {
            _logger.LogInformation("NoteController: Edit Called! ");
            var payload = new Payload<NoteEditRequest>();
            var command = new NoteEditCommand { NoteEditRequest = request };
            var validator = new NoteEditValidator();
            var state = await validator.ValidateAsync(request);

            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);

            return Ok(payload);
        }
        [HttpGet]
        [Route("IsEmployeeTag")]
        [ProducesResponseType(typeof(Payload<bool>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> IsEmployeeTag([Required] long noteId)
        {
            _logger.LogInformation("NoteController: IsEmployeeTag Called! ");
            var payload = await Mediator.Send(new IsNoteEmployeeTagQuery() { NoteId = noteId });
            return Ok(payload);
        }
        [HttpPost]
        [Route("PublicOKRSendNotificationsAndEmailsTagUser")]
        [ProducesResponseType(typeof(Payload<bool>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DraftToPublicUserNotificationsAndEmails(List<NoteDraftEmailRequest> goal)
        {
            _logger.LogInformation("NoteController: DraftToPublicUserNotificationsAndEmails Called! ");
            var command = new DraftToPublicUserCommand { Goal = goal };
            var payload = await Mediator.Send(command);
            return Ok(payload);
        }
        [HttpGet]
        [Route("UserNotes")]
        [ProducesResponseType(typeof(Payload<UserNoteResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetUserNotes(long goalId, int goalTypeId, int pageIndex = 1, int pageSize = 10)
        {
            _logger.LogInformation("NoteController: GetAll Called! ");
            var payload = await Mediator.Send(new UserNoteAllQuery() { GoalId = goalId, GoalTypeId = goalTypeId, PageIndex = pageIndex, PageSize = pageSize });
            return Ok(payload);
        }
    }
}