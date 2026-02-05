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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace OkrConversationService.Application.Controllers
{
    [Route("checkIn")]
    [ApiController]
    public class CheckInController : ApiControllerBase
    {
        private readonly ILogger<CheckInController> _logger;
        public CheckInController(ILoggerFactory loggerFactory, IMediator mediator, ICommonBase commonBase) : base(
           loggerFactory, mediator, commonBase)
        {
            _logger = LoggerFactory.CreateLogger<CheckInController>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(Payload<CheckInPointsResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAll(long empId = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            _logger.LogInformation("CheckInController: GetAll Called! ");
            var payload = await Mediator.Send(new CheckInGetAllQuery() { EmployeeId = empId, StartDate = startDate, EndDate = endDate });
            return Ok(payload);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Payload<CheckInDetailRequest>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Create(List<CheckInDetailRequest> request)
        {
            _logger.LogInformation("CheckInController: Create Called! ");
            var payload = new Payload<CheckInDetailRequest>();
            var command = new CheckInCreateCommand { CheckInDetailRequest = request };
            var validator = new CheckInCreateValidator();
            var state = await validator.ValidateAsync(request);
            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);
            return Ok(payload);
        }


        [HttpGet]
        [Route("CheckInWeeklyDates")]
        [ProducesResponseType(typeof(Payload<CheckInDatesPermissionResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllCheckInWeeklyDates(long empId = 0)
        {
            _logger.LogInformation("CheckInController: GetAllCheckInWeeklyDates Called! ");
            var payload = await Mediator.Send(new CheckInWeeklyDatesQuery() { EmployeeId = empId });
            return Ok(payload);
        }
        [HttpGet]
        [Route("directreports/{empId}")]
        [ProducesResponseType(typeof(Payload<DirectReportsResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDirectReports(long empId = 0)
        {
            _logger.LogInformation("CheckInController: GetDirectReports Called! ");
            var payload = await Mediator.Send(new AllDirectReportsEmployeeByIdQuery { EmpId = empId });
            return Ok(payload);
        }

        [HttpGet]
        [Route("IsCheckInSubmitted")]
        [ProducesResponseType(typeof(Payload<CheckInAlertResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> IsCheckInSubmitted()
        {
            _logger.LogInformation("CheckInController: IsCheckInSubmitted Called! ");
            var payload = await Mediator.Send(new IsCheckInSubmittedQuery() { });
            return Ok(payload);
        }
        [HttpPut]
        [ProducesResponseType(typeof(Payload<CheckInVisible>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateCheckinVisibility([Required] int checkInVisibilty)
        {
            _logger.LogInformation("CheckInController: UpdateCheckinVisibility Called! ");
            var payload = new Payload<CheckInVisible>();
            var command = new UpdateCheckinVisibilityCommand { CheckInVisibilty = (CheckInVisible)checkInVisibilty };
            var validator = new UpdateCheckinVisibilityValidator();
            var state = await validator.ValidateAsync((CheckInVisible)checkInVisibilty);
            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);
            return Ok(payload);
        }

        [HttpPost]
        [Route("Import")]
        [ProducesResponseType(typeof(Payload<bool>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> ImportPastTask()
        {
            _logger.LogInformation("CheckInController: ImportPastTask! ");
            var payload = await Mediator.Send(new ImportPastTaskCommand() { });
            return Ok(payload);
        }

        [HttpGet]
        [Route("DashboardCheckIn")]
        [ProducesResponseType(typeof(Payload<DashboardCheckInResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDashboardCheckInDetails(long empId)
        {
            _logger.LogInformation("CheckInController: GetDashboardCheckInDetails Called! ");
            var payload = await Mediator.Send(new CheckInDashboardQuery() { EmployeeId = empId });
            return Ok(payload);
        }

        [HttpDelete]
        [Route("employeecheckinvisible")]
        [ProducesResponseType(typeof(Payload<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteEmployeeCheckinVisible([Required] long employeeId)
        {
            _logger.LogInformation("DeleteEmployeeCheckinVisible: Delete Called! ");
            var payload = await Mediator.Send(new DeleteEmployeeCheckInVisibleCommand()
            {
                EmployeeId = employeeId
            });
            return Ok(payload);
        }
        [HttpGet]
        [Route("employeecheckinvisible")]
        [ProducesResponseType(typeof(Payload<EmployeeCheckInVisibleResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetEmployeeCheckInVisible(long employeeId, int pageIndex = 1, int pageSize = 10)
        {
            _logger.LogInformation("GetEmployeeCheckInVisible: Called! ");
            var payload = await Mediator.Send(new GetEmployeeCheckInVisibleQuery { EmpId = employeeId, PageIndex = pageIndex, PageSize = pageSize });
            return Ok(payload);
        }

        [HttpPost]
        [Route("addemployeecheckin")]
        [ProducesResponseType(typeof(Payload<bool>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> AddEmployeeCheckInVisible(EmployeeCheckInVisibleRequest request)

        {
            _logger.LogInformation("Recognition Create: Create Called! ");
            var payload = new Payload<bool>();
            var command = new EmployeeCheckInVisibleCommand { EmployeeCheckInVisibleRequest = request };
            var validator = new AddEmployeeCheckInVisibleValidator();
            var state = await validator.ValidateAsync(request);
            if (state.IsValid)
                payload = await Mediator.Send(command);
            else
                payload = CommonBase.GetPayloadStatus(payload, state.Errors);

            return Ok(payload);
        }

        [HttpGet]
        [Route("isaddedemployeecheckin")]
        [ProducesResponseType(typeof(Payload<DashboardCheckInResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> IsAddedEmployeeCheckIn(long empId)
        {
            _logger.LogInformation("CheckInController: IsAddedEmployeeCheckIn Called! ");
            var payload = await Mediator.Send(new IsAddedEmployeeCheckInQuery() { EmployeeId = empId });
            return Ok(payload);
        }

    }
}
