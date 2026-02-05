using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Domain.Validator;

namespace OkrConversationService.Application.Controllers
{
    [Route("recognition")]
    [ApiController]
    public class RecognitionController : ApiControllerBase
    {
        private readonly ILogger<RecognitionController> _logger;

        public RecognitionController(ILoggerFactory loggerFactory, IMediator mediator, ICommonBase commonBase) : base(
            loggerFactory, mediator, commonBase)
        {
            _logger = LoggerFactory.CreateLogger<RecognitionController>();
        }

        [HttpGet]
        [Route("RecognitionLike")]
        [ProducesResponseType(typeof(Payload<RecognitionReactionResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetRecognitionLike(long moduleDetailsId,int moduleId)
        {
            _logger.LogInformation("GetRecognitionLike: GetRecognitionLike Called! ");
            var payload = await Mediator.Send(new RecognitionLikeQuery() { ModuleDetailsId = moduleDetailsId,ModuleId = moduleId });
            return Ok(payload);
        }

        [HttpPost]
        [Route("Comments")]
        [ProducesResponseType(typeof(Payload<CommentDetailsRequest>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateComment(CommentDetailsRequest request)
        {
            _logger.LogInformation("CreateComment: CreateComment Called! ");
            var payload = new Payload<CommentDetailsRequest>();
            var command = new CommentCreateCommand { CommentDetailsRequest = request };
            var validator = new CommentCreateValidator();
            var state = await validator.ValidateAsync(request);
            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);

            return Ok(payload);
        }

        [HttpDelete]
        [Route("Comments")]
        [ProducesResponseType(typeof(Payload<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteComment([Required] long commentDetailsId)
        {
            _logger.LogInformation("DeleteComment: Delete Called! ");
            var payload = await Mediator.Send(new CommentDeleteCommand() { CommentDetailsId = commentDetailsId });
            return Ok(payload);
        }

        [HttpGet]
        [Route("Comments")]
        [ProducesResponseType(typeof(Payload<CommentResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetComments(long moduleDetailsId, int pageIndex, int pageSize,int moduleId)
        {
            _logger.LogInformation("GetComments: GetComments Called! ");
            var payload = await Mediator.Send(new GetCommentQuery() { ModuleDetailsId = moduleDetailsId, PageIndex = pageIndex, PageSize = pageSize,ModuleId = moduleId });
            return Ok(payload);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Payload<RecognitionCreateRequest>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Create(RecognitionCreateRequest request)

        {
            _logger.LogInformation("Recognition Create: Create Called! ");
            var payload = new Payload<RecognitionCreateRequest>();
            var command = new RecognitionCreateCommand { RecognitionRequest = request };
            var validator = new RecognitionCreateValidator();
            var state = await validator.ValidateAsync(request);
            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);

            return Ok(payload);
        }

        [HttpGet]
        [Route("badges")]
        [ProducesResponseType(typeof(Payload<RecognitionCategoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCategory(long empId)
        {
            _logger.LogInformation("GetCategory: GetCategory Called! ");
            var payload = await Mediator.Send(new RecognitionCategoryGetQuery() { EmployeeId = empId });
            return Ok(payload);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Payload<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete([Required] long recognitionId)
        {
            _logger.LogInformation("Delete: Delete Called! ");
            var payload = new Payload<bool>();
            var command = new RecognitionDeleteCommand { RecognitionId = recognitionId };
            var validator = new RecognitionDeleteValidator();
            var state = await validator.ValidateAsync(command);
            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);

            return Ok(payload);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Payload<RecognitionEditRequest>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Edit(RecognitionEditRequest request)
        {
            _logger.LogInformation("Recognition Edit: Edit Called! ");
            var payload = new Payload<RecognitionEditRequest>();
            var command = new RecognitionEditCommand { RecognitionEditRequest = request };
            var validator = new RecognitionEditValidator();
            var state = await validator.ValidateAsync(request);
            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);

            return Ok(payload);
        }

        [HttpPost]
        [Route("OrgRecognition")]
        [ProducesResponseType(typeof(Payload<OrgRecognitionResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetOrgRecognition(OrgRecognitionRequest request)
        {
            _logger.LogInformation("OrgRecognition: GetComments Called! ");
            var payload = await Mediator.Send(new GetOrgRecognitionQuery() { Id = request.id, SearchType = request.searchType, IsMyPost = request.isMyPost, PageIndex = request.pageIndex, PageSize = request.pageSize, RecognitionId = request.RecognitionId });

            return Ok(payload);
        }
        //OP-12364

        [HttpPost]
        [Route("totalrecognitionbyteamid")]
        [ProducesResponseType(typeof(Payload<TotalRecognitionByTeamIdResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> TotalRecognitionByTeamId(RecognitionTeamRequest request)
        {
            _logger.LogInformation("Recognition: TotalRecognitionByTeamId Called! ");
            var payload = await Mediator.Send(new TotalRecognitionByTeamIdGetQuery()
            {
                Team = new TotalRecognitionByTeamIdRequest
                {
                    Id = request.TeamId
                }
            });
            return Ok(payload);
        }
        #region LeaderBoard
        [HttpPost]
        [Route("employeesleaderboard")]
        [ProducesResponseType(typeof(Payload<RecognitionByTeamIdResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> EmployeesLeaderBoard(EmployeesLeaderBoardRequest request)
        {
            _logger.LogInformation("myteamleaderboard: GetCategory Called! ");          
            var payload = await Mediator.Send(new EmployeesLeaderBoardGetQuery()
            {
                Request = request

            });
            return Ok(payload);
        }


        [HttpPost]
        [Route("teamsleaderboard")]
        [ProducesResponseType(typeof(Payload<RecognitionTeamsResponse>), (int)HttpStatusCode.OK)] //RecognitionByemployeeIdResponse
        public async Task<ActionResult> TeamsLeaderBoard(RecognitionLeaderBoardRequest request) //RecognitionEmployeeRequest
        {
            _logger.LogInformation("recognitionteams: recognitionteams Called! ");          
            var payload = await Mediator.Send(new TeamsLeaderBoardGetQuery()
            {
                Request = request

            });
            return Ok(payload);
        }

        #endregion
        [HttpGet]
        [Route("getteamsbyempid")]
        [ProducesResponseType(typeof(Payload<RecognitionByTeamIdResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTeamsByEmpId(long empId)
        {
            _logger.LogInformation("getteamsbyempid: GetTeamsByEmpId Called! ");

            var payload = await Mediator.Send(new TeamsByEmpIdGetQuery()
            {
                EmployeeId = empId

            });
            return Ok(payload);
        }

        [HttpGet]
        [Route("GetRecognitionById")]
        [ProducesResponseType(typeof(Payload<RecognitionByTeamIdResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetRecognitionId(long id)
        {
            _logger.LogInformation("GetRecognitionId: GetRecognitionId Called! ");

            var payload = await Mediator.Send(new GetRecognitionByIdQuery()
            {
                RecognitionId = id
            });
            return Ok(payload);
        }

        #region WallofFame
        [HttpGet]
        [Route("mywalloffamedashboard")]
        [ProducesResponseType(typeof(Payload<MyWallOfFameResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> MyWallOfFameDashBoard()
        {
            _logger.LogInformation("MyWallOfFameDashBoard: GetCategory Called! ");

            var payload = await Mediator.Send(new MyWallOfFameDashBoardGetQuery()
            {

            });
            return Ok(payload);
        }
        [HttpPost]
        [Route("mywalloffame")]
        [ProducesResponseType(typeof(Payload<MyWallOfFameResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetMyWallOfFame(MyWallOfFameRequest request)
        {
            _logger.LogInformation("GetMyWallOfFame: GetCategory Called! ");
            var payload = await Mediator.Send(new MyWallOfFameGetQuery()
            {
                MyMyWallOfFameRequest = request
            });
            return Ok(payload);


        }
        #endregion


        [HttpPost]
        [Route("GetRecognitionForWall")]
        [ProducesResponseType(typeof(Payload<RecognitionDetailsResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetRecognitionForWall(RecognitionForWallRequest request)
        {
            _logger.LogInformation("OrgRecognition: GetComments Called! ");
            var payload = await Mediator.Send(new GetRecognitionForWallQuery() {  StartDate = request.startDate, EndDate = request.endDate,  PageIndex = request.pageIndex, PageSize = request.pageSize, emailId = request.emailId });
            return Ok(payload);
        }
    }
}
