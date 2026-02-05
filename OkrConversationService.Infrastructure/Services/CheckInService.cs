using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Services
{
    public class CheckInService : BaseService, ICheckInService
    {
        private readonly IRepositoryAsync<CheckInPoint> _checkInPointRepo;
        private readonly IRepositoryAsync<CheckInDetail> _checkInDetailRepo;
        private readonly IRepositoryAsync<Employee> _employeeRepo;
        private readonly IRepositoryAsync<Constant> _constantRepo;
        private readonly IRepositoryAsync<TeamSetting> _teamSettingRepo;
        private readonly IRepositoryAsync<CheckInEmployeeMapping> _checkInEmployeeMappingRepo;
        private readonly ICommonService _commonService;
        private readonly IRepositoryAsync<TaskDetail> _taskDetailRepo;
        private readonly IRepositoryAsync<GoalKey> _goalKeyRepo;
        private readonly IRepositoryAsync<EmployeeCheckInVisiblePermissions> _employeeCheckInVisiblePermissionsRepo;

        [Obsolete("")]
        public CheckInService(IServicesAggregator servicesAggregateService, ICommonService commonService) : base(servicesAggregateService)
        {
            _checkInPointRepo = UnitOfWorkAsync.RepositoryAsync<CheckInPoint>();
            _checkInDetailRepo = UnitOfWorkAsync.RepositoryAsync<CheckInDetail>();
            _employeeRepo = UnitOfWorkAsync.RepositoryAsync<Employee>();
            _constantRepo = UnitOfWorkAsync.RepositoryAsync<Constant>();
            _teamSettingRepo = UnitOfWorkAsync.RepositoryAsync<TeamSetting>();
            _checkInEmployeeMappingRepo = UnitOfWorkAsync.RepositoryAsync<CheckInEmployeeMapping>();
            _commonService = commonService;
            _taskDetailRepo = UnitOfWorkAsync.RepositoryAsync<TaskDetail>();
            _goalKeyRepo = UnitOfWorkAsync.RepositoryAsync<GoalKey>();
            _employeeCheckInVisiblePermissionsRepo = UnitOfWorkAsync.RepositoryAsync<EmployeeCheckInVisiblePermissions>();

        }

        public async Task<Payload<CheckInPointsResponse>> GetAll(CheckInGetAllQuery request)
        {
            var payload = new Payload<CheckInPointsResponse>() { Entity = new CheckInPointsResponse() };
            var userIdentity = _commonService.GetUserIdentity();
            DateTime releaseDate = Convert.ToDateTime("12-08-2022").Date;
            bool isOldVersion = false;
            //check for user permission
            if (!IsCheckinVisible(request.EmployeeId, userIdentity.EmployeeId))
                return GetPayloadStatusSuccess(payload, "message", ResourceMessage.CheckinVisibleByReportingManagerOnly);

            var empId = request.EmployeeId > 0 ? request.EmployeeId : userIdentity.EmployeeId;
            var startDate = request.StartDate ?? await GetLastStartDate();
            var endDate = request.EndDate ?? await GetNextEndDate();

            if (releaseDate >= startDate)
            {
                isOldVersion = true;
            }
            var checkIn = await (from check in _checkInPointRepo.GetQueryable().Where(x => x.IsActive)
                                 join det in _checkInDetailRepo.GetQueryable().Where(x => x.EmployeeId == empId && x.CheckInDate.Date >= startDate.Date && x.CheckInDate.Date <= endDate.Date && x.IsActive)
                                 on check.CheckInPointsId equals det.CheckInPointsId into checkInDet
                                 from checkInDetails in checkInDet.DefaultIfEmpty()
                                 select new CheckInDetailsResponse
                                 {
                                     CheckInPointsId = check.CheckInPointsId,
                                     CheckInPoints = check.CheckInPoints,
                                     CheckInDetailsId = checkInDetails != null ? checkInDetails.CheckInDetailsId : 0,
                                     CheckInDetails = checkInDetails != null ? checkInDetails.CheckInDetails : string.Empty,
                                     CheckInDate = checkInDetails != null ? checkInDetails.CheckInDate : startDate.Date,
                                     IsAfterCycleCutoff = checkInDetails != null,///(checkInDetails.UpdatedOn.HasValue) ? checkInDetails.UpdatedOn.Value.Date >= endDate.Date : checkInDetails.CreatedOn.Date >= endDate.Date,
                                     CheckInSubmitDate = checkInDetails != null ? checkInDetails.UpdatedOn ?? checkInDetails.CreatedOn : (DateTime?)null

                                 }).ToListAsync();

            var taskDetails = GetAllTaskDetails(empId, startDate, endDate);

            //update questions for future week
            if (startDate.Date > DateTime.Now.Date)
            {
                checkIn.RemoveAll(s => s.CheckInPointsId == 2);
                checkIn.FirstOrDefault(x => x.CheckInPointsId == 1).CheckInPoints = AppConstants.CheckInFutureWeekFirstQuestion;
                checkIn.FirstOrDefault(x => x.CheckInPointsId == 4).CheckInPoints = AppConstants.CheckInFutureWeekLastQuestion;
            }

            if (!isOldVersion)
                checkIn = checkIn.Where(x => x.CheckInPointsId != 3).ToList();


            payload.Entity = new CheckInPointsResponse { IsOldVersion = isOldVersion, CheckInDetailsResponse = checkIn, TaskResponse = taskDetails };
            payload = GetPayloadStatusSuccess(payload);

            return payload;
        }

        public async Task<Payload<CheckInDetailRequest>> Create(CheckInCreateCommand request)
        {
            var userIdentity = _commonService.GetUserIdentity();
            var payload = new Payload<CheckInDetailRequest>();
            var checkInIdList = request.CheckInDetailRequest.Select(x => x.CheckInDetailsId).ToList();
            var createDetails = _checkInDetailRepo.GetQueryable().Where(x => checkInIdList.Contains(x.CheckInDetailsId)).ToList();
            var checkInDetailsJson = JsonConvert.SerializeObject(createDetails);
            foreach (var item in request.CheckInDetailRequest)
            {
                var checkIndetail = createDetails.FirstOrDefault(x => x.CheckInDetailsId == item.CheckInDetailsId);
                if (checkIndetail == null)
                {
                    var checkInDetail = new CheckInDetail
                    {
                        CheckInPointsId = item.CheckInPointsId,
                        EmployeeId = item.EmployeeId,
                        CheckInDetails = item.CheckInDetails ?? "",
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = request.CreatedOn,
                        CheckInDate = item.CheckInDate

                    };
                    _checkInDetailRepo.Add(checkInDetail);
                }
                else
                {
                    checkIndetail.CheckInDetails = item.CheckInDetails ?? "";
                    checkIndetail.UpdatedBy = userIdentity.EmployeeId;
                    checkIndetail.UpdatedOn = request.UpdatedOn;
                    _checkInDetailRepo.Update(checkIndetail);
                }
            }
            await UnitOfWorkAsync.SaveChangesAsync();

            // Call signalR to broadcast
            var signalrRequestModel = new SignalrRequestModel()
            {
                BroadcastValue = request.CheckInDetailRequest.Select(x => x.EmployeeId).ToList(),
                BroadcastTopic = AppConstants.TopicCheckInUpdate
            };
            if (signalrRequestModel.BroadcastValue.Count > 0)
                await _commonService.CallSignalRFunctionForContributors(signalrRequestModel);

            payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.CheckInCreated);

            // Impersonate Audit Log            
            if (userIdentity.IsImpersonatedUser)
            {
                var checkInUpdatedDetailsJson = JsonConvert.SerializeObject(request.CheckInDetailRequest);
                AuditLogRequest auditLogRequest = new AuditLogRequest
                {
                    ActivityName = AppConstants.AddCheckIn,
                    ActivityDescription = "CheckIn Created/Updated - " + checkInUpdatedDetailsJson,
                    TransactionId = 0,
                    ActivityPreviousData = checkInDetailsJson
                };
                await _commonService.ImpersonateAuditLog(auditLogRequest);
            }

            //call AuditEngagementReport
            await _commonService.AuditEngagementReport(new CreateEngagementReportRequest
            {
                EmployeeId = userIdentity.EmployeeId,
                EngagementTypeId = AppConstants.Engagement_CheckIn
            }).ConfigureAwait(false);

            return payload;

        }

        public async Task<Payload<CheckInDatesPermissionResponse>> GetAllCheckInWeeklyDates(CheckInWeeklyDatesQuery request)
        {
            var payload = new Payload<CheckInDatesPermissionResponse>();
            var userIdentity = _commonService.GetUserIdentity();

            var empId = request.EmployeeId > 0 ? request.EmployeeId : _commonService.GetUserIdentity().EmployeeId;
            var checkIn = await CalculateEmployeeWeeksAndStatus(empId, AppConstants.TwentyFiveWeek, true);
            if (checkIn.Count > 0)
            {
                payload.Entity = new CheckInDatesPermissionResponse()
                {
                    CheckInWeeklyDatesResponse = checkIn,
                    IsCheckinDataVisible = IsCheckinVisible(request.EmployeeId, userIdentity.EmployeeId)
                };
                payload = GetPayloadStatusSuccess(payload);
            }
            else
            {
                payload = GetPayloadStatusNoContent(payload, "message", ResourceMessage.RecordNotFoundMessage);
            }
            return payload;
        }

        public async Task<Payload<DirectreportsResponseResult>> GetAllDirectReportsByIds(AllDirectReportsEmployeeByIdQuery request)
        {
            var payload = new Payload<DirectreportsResponseResult>() { Entity = new DirectreportsResponseResult() };

            //var objectStatus = new List<DirectreportsResponseResult>();
            var DirectsStatus = new List<DirectReportsResponse>();
            var OthersStatus = new List<DirectReportsResponse>();
            var empId = request.EmpId > 0 ? request.EmpId : _commonService.GetUserIdentity().EmployeeId;
            var employees = await _employeeRepo.GetQueryable()
                    .Where(x => x.IsActive && x.ReportingTo == empId).ToListAsync();
            var userIdentity = _commonService.GetUserIdentity();
            //var visibileEmployees = (from visbleEmp in _employeeCheckInVisiblePermissionsRepo.GetQueryable()
            //    .Where(x => x.IsActive && (x.UpdatedBy.HasValue ? x.UpdatedBy.Value == empId : x.CreatedBy == empId))
            //                         join emp in _employeeRepo.GetQueryable().Where(y => y.IsActive) on visbleEmp.EmployeeId equals emp.EmployeeId
            //                         select new { emp.FirstName, emp.LastName, emp.ImagePath, emp.Designation, emp.EmployeeId }).ToList();
            var visibileEmployees = (from visbleEmp in _employeeCheckInVisiblePermissionsRepo.GetQueryable()
               .Where(x => x.IsActive && x.EmployeeId == userIdentity.EmployeeId)
                                     join emp in _employeeRepo.GetQueryable().Where(y => y.IsActive) on visbleEmp.CreatedBy equals emp.EmployeeId
                                     select new { emp.FirstName, emp.LastName, emp.ImagePath, emp.Designation, emp.EmployeeId }).ToList();


            foreach (var emp in employees)
            {
                var addObjectStatus = new DirectReportsResponse
                {
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    ImagePath = emp.ImagePath,
                    Designation = emp.Designation,
                    EmployeeId = emp.EmployeeId,
                    CheckInStatus = await GetFourCheckInStatus(emp.EmployeeId)
                };
                DirectsStatus.Add(addObjectStatus);
            }

            foreach (var vEmp in visibileEmployees)
            {
                var addObjectStatus = new DirectReportsResponse
                {
                    FirstName = vEmp.FirstName,
                    LastName = vEmp.LastName,
                    ImagePath = vEmp.ImagePath,
                    Designation = vEmp.Designation,
                    EmployeeId = vEmp.EmployeeId,
                    CheckInStatus = await GetFourCheckInStatus(vEmp.EmployeeId)
                };
                OthersStatus.Add(addObjectStatus);
            }

            payload.Entity = new DirectreportsResponseResult { DirectReports = DirectsStatus, OtherReports = OthersStatus };
            payload = GetPayloadStatusSuccess(payload);
            return payload;
        }

        public async Task<Payload<CheckInAlertResponse>> IsCheckInSubmitted()
        {
            var payload = new Payload<CheckInAlertResponse>() { Entity = new CheckInAlertResponse() };
            var empId = _commonService.GetUserIdentity().EmployeeId;
            var startDate = await GetLastStartDate();
            var endDate = await GetNextEndDate();
            var checkInDetailCount = await _checkInDetailRepo.GetQueryable().CountAsync(x => x.IsActive && x.EmployeeId == empId && x.CheckInDate.Date >= startDate.Date && x.CheckInDate.Date <= endDate.Date);

            var taskDetailsCount = await _taskDetailRepo.GetQueryable().CountAsync(x => x.IsActive && x.TaskType == (int)EnumTaskType.CheckIn
                              && x.CreatedBy == empId && x.CreatedOn.Date >= startDate.Date && x.CreatedOn.Date <= endDate.Date);

            var totalDays = (endDate.Date - DateTime.UtcNow.Date).TotalDays + 1;//days differance should be 7 instead of 6 including current date also
            var checkInSettings = CheckInVisibilty(empId);
            var checkInAlert = new CheckInAlertResponse()
            {
                IsAlert = checkInDetailCount == 0 && taskDetailsCount == 0 && totalDays <= 2,
                RemainingDays = CheckInRemainingDays(checkInDetailCount, taskDetailsCount, totalDays),
                CheckInVisibilty = checkInSettings.Item1,
                IsChangeCheckInVisibilty = checkInSettings.Item2,
                IsImportButtonVisible = IsImportButtonVisible(empId)
            };

            payload.Entity = checkInAlert;
            return payload;
        }

        public async Task<Tuple<int, int>> GetConstantCheckInEndDateInDay()
        {
            var constant = await _constantRepo.GetQueryable().Where(x => x.IsActive.Value).ToListAsync();
            var checkInEndDateInDay = constant.FirstOrDefault(x => x.ConstantName == AppConstants.CheckInEndDateInDay);

            var checkInFutureWeekDisplayCount = constant.FirstOrDefault(x => x.ConstantName == AppConstants.CheckInFutureWeekDisplayCount);
            return Tuple.Create(Convert.ToInt32(checkInEndDateInDay.ConstantValue), Convert.ToInt32(checkInFutureWeekDisplayCount.ConstantValue));
        }

        public async Task<Payload<CheckInVisible>> UpdateCheckinVisibilityList(UpdateCheckinVisibilityCommand request)
        {
            var payload = new Payload<CheckInVisible>() { Entity = request.CheckInVisibilty };
            var userIdentity = _commonService.GetUserIdentity();
            var isCheckInVisibilty = CheckInVisibilty(userIdentity.EmployeeId);
            if (isCheckInVisibilty.Item2)
            {
                var checkinEmployeeMappings = await _checkInEmployeeMappingRepo.GetQueryable().Where(x => x.EmployeeId == userIdentity.EmployeeId).ToListAsync();
                foreach (var checkinEmployeeMapping in checkinEmployeeMappings)
                {
                    if (checkinEmployeeMapping != null)
                    {
                        checkinEmployeeMapping.IsActive = request.IsActive;
                        checkinEmployeeMapping.CheckInVisibilty = (int)request.CheckInVisibilty;
                        checkinEmployeeMapping.UpdatedOn = request.UpdatedOn;
                        _checkInEmployeeMappingRepo.Update(checkinEmployeeMapping);
                    }
                    else
                    {
                        _checkInEmployeeMappingRepo.Add(new CheckInEmployeeMapping()
                        {
                            CheckInVisibilty = (int)request.CheckInVisibilty,
                            EmployeeId = userIdentity.EmployeeId,
                            IsActive = request.IsActive,
                            CreatedOn = request.CreatedOn,
                            UpdatedOn = request.UpdatedOn
                        });
                    }
                }

                await UnitOfWorkAsync.SaveChangesAsync();
                payload = GetPayloadStatusSuccess(payload, "message", ResourceMessage.CheckInVisibalityUpdatedMsg);
            }
            else
                payload = GetPayloadStatus(payload, "message", ResourceMessage.UserCantUpdateCheckInVisibality);

            return payload;
        }
        public async Task<Payload<CheckInVisible>> UpdateCheckinVisibility(UpdateCheckinVisibilityCommand request)
        {
            var payload = new Payload<CheckInVisible>() { Entity = request.CheckInVisibilty };
            var userIdentity = _commonService.GetUserIdentity();
            var isCheckInVisibilty = CheckInVisibilty(userIdentity.EmployeeId);
            if (isCheckInVisibilty.Item2)
            {
                var checkinEmployeeMapping = await _checkInEmployeeMappingRepo.GetQueryable().FirstOrDefaultAsync(x => x.EmployeeId == userIdentity.EmployeeId);
                if (checkinEmployeeMapping != null)
                {
                    checkinEmployeeMapping.IsActive = request.IsActive;
                    checkinEmployeeMapping.CheckInVisibilty = (int)request.CheckInVisibilty;
                    checkinEmployeeMapping.UpdatedOn = request.UpdatedOn;
                    _checkInEmployeeMappingRepo.Update(checkinEmployeeMapping);
                }
                else
                {
                    _checkInEmployeeMappingRepo.Add(new CheckInEmployeeMapping()
                    {
                        CheckInVisibilty = (int)request.CheckInVisibilty,
                        EmployeeId = userIdentity.EmployeeId,
                        IsActive = request.IsActive,
                        CreatedOn = request.CreatedOn,
                        UpdatedOn = request.UpdatedOn
                    });
                }
                await UnitOfWorkAsync.SaveChangesAsync();
                payload = GetPayloadStatusSuccess(payload, "message", ResourceMessage.CheckInVisibalityUpdatedMsg);
            }
            else
                payload = GetPayloadStatus(payload, "message", ResourceMessage.UserCantUpdateCheckInVisibality);

            return payload;
        }
        public async Task<Payload<bool>> ImportPastTask(ImportPastTaskCommand request)
        {
            var userIdentity = _commonService.GetUserIdentity();
            var payload = new Payload<bool> { Entity = false };
            var pastCheckIn = await CalculateEmployeeWeeksAndStatus(userIdentity.EmployeeId, AppConstants.ImportPastTaskWeek, false);
            if (pastCheckIn.Count > 0)
            {
                var pastDate = pastCheckIn.OrderBy(x => x.StartDate).FirstOrDefault();
                var pastTaskList = GetNoCompletedPastTask(userIdentity.EmployeeId, pastDate.StartDate, pastDate.EndDate).Where(x => !x.IsCompleted).ToList();
                if (pastTaskList.Count > 0)
                {
                    foreach (var item in pastTaskList)
                    {
                       
                        item.IsImported = true;
                        item.CreatedOn = item.CreatedOn.AddDays(7);
                        item.IsActive = request.IsActive;
                        item.DueDate = item.CreatedOn.AddDays(6);
                        item.UpdatedBy = userIdentity.EmployeeId;
                        item.UpdatedOn = DateTime.UtcNow;
                        _taskDetailRepo.Update(item);
                      
                    }
                    payload.Entity = true;
                    await UnitOfWorkAsync.SaveChangesAsync();
                    payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.ImportPastTask);
                }
                // Impersonate Audit Log            
                if (userIdentity.IsImpersonatedUser)
                {
                    AuditLogRequest auditLogRequest = new AuditLogRequest
                    {
                        ActivityName = AppConstants.ImportPastTask,
                        ActivityDescription = "Past Task Imported",
                        TransactionId = 0
                    };
                    await _commonService.ImpersonateAuditLog(auditLogRequest);
                }

            }
            return payload;
        }

        public async Task<Payload<DashboardCheckInResponse>> GetDashboardCheckInDetails(CheckInDashboardQuery request)
        {
            var payload = new Payload<DashboardCheckInResponse> { };
            var startDate = await GetLastStartDate();
            var endDate = await GetNextEndDate();
            var checkInDetail = await _checkInDetailRepo.GetQueryable().CountAsync(x => x.IsActive && x.EmployeeId == request.EmployeeId && x.CheckInDate.Date >= startDate.Date && x.CheckInDate.Date <= endDate.Date);

            ///Get pending task count
            var taskCount = await _taskDetailRepo.GetQueryable().CountAsync(x => x.IsActive && x.CreatedBy == request.EmployeeId
            && x.CreatedOn.Date >= DateTime.UtcNow.AddMonths(-3) && x.CreatedOn.Date <= DateTime.UtcNow && !x.IsCompleted);

            var taskDetailsCount = await _taskDetailRepo.GetQueryable().CountAsync(x => x.IsActive && x.TaskType == (int)EnumTaskType.CheckIn
                             && x.CreatedBy == request.EmployeeId && x.CreatedOn.Date >= startDate.Date && x.CreatedOn.Date <= endDate.Date);

            var totalDays = (endDate.Date - DateTime.UtcNow.Date).TotalDays + 1;//days differance should be 7 instead of 6 including current date also
            var checkIn = await GetFourCheckInStatus(request.EmployeeId);
            var checkInSort = checkIn.OrderByDescending(x => x.StartDate).ToList();
            checkInSort.RemoveAt(2);
            checkInSort.RemoveAt(2);
            var dashboardCheckIn = new DashboardCheckInResponse
            {
                IsAlert = checkInDetail == 0 && taskDetailsCount == 0 && totalDays <= 2,
                RemainingDays = CheckInRemainingDays(checkInDetail, taskDetailsCount, totalDays),
                TaskCount = taskCount,
                DisplayDate = startDate.ToString("MMM") + " " + startDate.Day + " - " + endDate.ToString("MMM") + " " + endDate.Day,
                CheckInStatus = checkInSort,//checkIn.OrderByDescending(x => x.StartDate).ToList(),
                RemaingDaysLeft = totalDays,
                IsCheckInSubmitted = checkInDetail > 0 || taskDetailsCount > 0
            };
            payload.Entity = dashboardCheckIn;
            payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.CheckInDashboard);
            return payload;
        }

        public async Task<Payload<bool>> DeleteEmployeeCheckinVisible(DeleteEmployeeCheckInVisibleCommand request)
        {
            var userIdentity = _commonService.GetUserIdentity();
            var payload = new Payload<bool> { Entity = false };
            var getVisibleEmplyee = await _employeeCheckInVisiblePermissionsRepo.GetQueryable()
               .FirstOrDefaultAsync(x => x.IsActive && x.CreatedBy == userIdentity.EmployeeId
                 && x.EmployeeId == request.EmployeeId);
            if (getVisibleEmplyee == null)
                return GetPayloadStatus(payload, "message", ResourceMessage.RecordNotFoundMessage);
            else
            {
                getVisibleEmplyee.IsActive = false;
                getVisibleEmplyee.UpdatedBy = userIdentity.EmployeeId;
                getVisibleEmplyee.UpdatedOn = request.UpdatedOn;
                _employeeCheckInVisiblePermissionsRepo.Update(getVisibleEmplyee);
                await UnitOfWorkAsync.SaveChangesAsync();
                payload.Entity = true;
                payload.MessageList.Add("message", ResourceMessage.RecordDeleted);
                payload.Status = (int)System.Net.HttpStatusCode.OK;

            }
            return payload;
        }

        public async Task<Payload<EmployeeCheckInVisibleResponse>> GetEmployeeCheckInVisible(GetEmployeeCheckInVisibleQuery request)
        {
            var payload = new Payload<EmployeeCheckInVisibleResponse>() { Entity = new EmployeeCheckInVisibleResponse() };
            var objectStatus = new List<EmployeeCheckInVisible>();
            var empId = request.EmpId > 0 ? request.EmpId : _commonService.GetUserIdentity().EmployeeId;
            var employees = await _employeeRepo.GetQueryable().Where(x => x.IsActive && x.ReportingTo
              == empId || x.EmployeeId == empId).ToListAsync();

            //reportingMgr
            var empReportingTo = employees.FirstOrDefault(y => y.EmployeeId == empId);
            if (empReportingTo != null && empReportingTo.ReportingTo.HasValue)
            {
                var reportingMgr = await _employeeRepo.GetQueryable().FirstOrDefaultAsync(x => x.IsActive && x.EmployeeId == empReportingTo.ReportingTo.Value);
                if (reportingMgr != null)
                    objectStatus.Add(new EmployeeCheckInVisible
                    {
                        Designation = reportingMgr.Designation,
                        FirstName = reportingMgr.FirstName,
                        EmailId = reportingMgr.EmailId,
                        LastName = reportingMgr.LastName,
                        ImagePath = reportingMgr.ImagePath,
                        EmployeeId = reportingMgr.EmployeeId,
                        CreatedDate = reportingMgr.CreatedOn,
                        IsReportingManger = true
                    });
            }

            //visible Employees List
            var visibleEmployees = (from visbleEmp in _employeeCheckInVisiblePermissionsRepo.GetQueryable()
                .Where(x => x.IsActive && x.CreatedBy == empId)
                                    join emp in _employeeRepo.GetQueryable().Where(y => y.IsActive) on visbleEmp.EmployeeId equals emp.EmployeeId
                                    select new EmployeeCheckInVisible
                                    {
                                        FirstName = emp.FirstName,
                                        LastName = emp.LastName,
                                        EmailId = emp.EmailId,
                                        ImagePath = emp.ImagePath,
                                        Designation = emp.Designation,
                                        EmployeeId = emp.EmployeeId,
                                        CreatedDate = emp.CreatedOn,
                                        IsReportingManger = false
                                    }).ToList();
            if (visibleEmployees.Any())
            {
                objectStatus.AddRange(visibleEmployees);

            }
            var checkinEmployeeMappings = await _checkInEmployeeMappingRepo.GetQueryable()
               .FirstOrDefaultAsync(x => x.EmployeeId == request.EmpId);

            request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
            var skipAmount = request.PageSize * (request.PageIndex - 1);
            var totalRecords = objectStatus.Count;
            var totalPages = totalRecords / request.PageSize;

            if (totalRecords % request.PageSize > 0)
                totalPages++;
            var result = new PageInfo
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };

            if (totalRecords % request.PageSize > 0)
                totalPages = totalPages + 1;

            payload.Entity = new EmployeeCheckInVisibleResponse
            {
                EmployeeCheckInVisibles = objectStatus.Skip(skipAmount)
                           .Take(request.PageSize).ToList(),
                CheckInVisibilty = checkinEmployeeMappings != null ? checkinEmployeeMappings.CheckInVisibilty : 1
            };
            payload.PagingInfo = result;
            payload = GetPayloadStatusSuccess(payload);
            return payload;
        }

        public async Task<Payload<bool>> AddEmployeeCheckInVisible(EmployeeCheckInVisibleCommand request)
        {

            var payload = new Payload<bool>() { Entity = new bool { } };
            var userIdentity = _commonService.GetUserIdentity();
            var employeeIds = request.EmployeeCheckInVisibleRequest.EmployeeIds.Select(x => x.EmployeeId).Distinct().ToList();
            var checkInVisibilty = request.EmployeeCheckInVisibleRequest.CheckInVisibilty;
            var visibileEmployees = await _employeeCheckInVisiblePermissionsRepo.GetQueryable()
                 .Where(x => x.IsActive && x.CreatedBy == userIdentity.EmployeeId
                 && employeeIds.Contains(x.EmployeeId)).ToListAsync();
            var checkinEmployeeMappings = await _checkInEmployeeMappingRepo.GetQueryable()
                 .Where(x => employeeIds.Contains(x.EmployeeId)).ToListAsync();
            foreach (var iemployeeId in employeeIds)
            {
                if (visibileEmployees.FirstOrDefault(x => x.EmployeeId == iemployeeId) == null)
                {
                    EmployeeCheckInVisiblePermissions empCheckinVisiable = new EmployeeCheckInVisiblePermissions()
                    {
                        EmployeeId = iemployeeId,
                        IsActive = true,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = DateTime.UtcNow,

                    };
                    await _employeeCheckInVisiblePermissionsRepo.AddAsync(empCheckinVisiable);
                }

                //CheckinVisibility Add
                var checkinEmployeeMapping = checkinEmployeeMappings.FirstOrDefault(x => x.EmployeeId == iemployeeId);
                var currentRequest = request.EmployeeCheckInVisibleRequest;
                if (checkinEmployeeMapping != null)
                {

                    checkinEmployeeMapping.IsActive = request.IsActive;
                    checkinEmployeeMapping.CheckInVisibilty = (int)currentRequest.CheckInVisibilty;
                    checkinEmployeeMapping.UpdatedOn = request.UpdatedOn;
                    _checkInEmployeeMappingRepo.Update(checkinEmployeeMapping);
                }
                else
                {
                    _checkInEmployeeMappingRepo.Add(new CheckInEmployeeMapping()
                    {
                        CheckInVisibilty = (int)currentRequest.CheckInVisibilty,
                        EmployeeId = iemployeeId,
                        IsActive = request.IsActive,
                        CreatedOn = request.CreatedOn,
                        UpdatedOn = request.UpdatedOn
                    });
                }

            }
            await UpdateCheckinVisibility(new UpdateCheckinVisibilityCommand
            {
                CheckInVisibilty = (CheckInVisible)checkInVisibilty,//(CheckInVisible)(checkInVisibilty! == 1 || checkInVisibilty! == 2 ? 1 : checkInVisibilty),
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                UpdatedOn = DateTime.UtcNow
            });
            var result = await UnitOfWorkAsync.SaveChangesAsync();
            payload.MessageList.Add("message", ResourceMessage.RecordAdded);
            payload.Entity = result.Success;
            payload.IsSuccess = true;
            payload.Status = (int)System.Net.HttpStatusCode.OK;
            payload = GetPayloadStatusSuccess(payload);
            return payload;
        }

        public async Task<Payload<IsAddedEmployeeCheckInResponse>> IsAddedEmployeeCheckIn(IsAddedEmployeeCheckInQuery request)
        {
            var payload = new Payload<IsAddedEmployeeCheckInResponse>() { Entity = new IsAddedEmployeeCheckInResponse() };
            var loginEmployeeId = _commonService.GetUserIdentity().EmployeeId;

            var empId = request.EmployeeId > 0 ? request.EmployeeId : loginEmployeeId;
            var employees = await _employeeRepo.GetQueryable().Where(x => x.IsActive && x.EmployeeId == loginEmployeeId).ToListAsync();

            //reportingMgr
            if (loginEmployeeId == empId)
            {
                payload.Entity.IsAdded = false;
                payload = GetPayloadStatus(payload, "message", ResourceMessage.CanNotAddedSelf);
                payload.Status = (int)HttpStatusCode.OK;
                return payload;
            }
            var empReportingTo = employees.FirstOrDefault(y => y.EmployeeId == loginEmployeeId);
            if (empReportingTo != null && empReportingTo.ReportingTo.HasValue && empReportingTo.ReportingTo.Value == request.EmployeeId)
            {
                payload.Entity.IsAdded = false;
                payload = GetPayloadStatus(payload, "message", ResourceMessage.CanNotAddedReportingManger);
                payload.Status = (int)HttpStatusCode.OK;
                return payload;
            }
            var user = await _employeeCheckInVisiblePermissionsRepo.GetQueryable()
               .FirstOrDefaultAsync(x => x.IsActive && x.CreatedBy == loginEmployeeId && x.EmployeeId == empId);
            if (user != null)
            {
                payload.Entity.IsAdded = false;
                payload = GetPayloadStatus(payload, "message", ResourceMessage.AlreadyAdded);
                payload.Status = (int)HttpStatusCode.OK;
                return payload;
            }

            payload = GetPayloadStatusSuccess(payload);
            payload.Entity.IsAdded = true;
            return payload;
        }

        #region Private

        private bool IsImportButtonVisible(long empId)
        {
            var pastCheckIn = CalculateEmployeeWeeksAndStatus(empId, AppConstants.ImportPastTaskWeek).Result;
            if (pastCheckIn.Count > 0)
            {
                var pastDate = pastCheckIn.OrderBy(x => x.StartDate).FirstOrDefault();
                var pastTaskList = GetNoCompletedPastTask(empId, pastDate.StartDate, pastDate.EndDate).Where(x => !x.IsCompleted).ToList();
                return pastTaskList.Count > 0;
            }
            return true;
        }

        private List<TaskResponse> GetAllTaskDetails(long empId, DateTime startDate, DateTime endDate)
        {
            var tasks = (from task in _taskDetailRepo.GetQueryable().Where(x => x.IsActive && x.TaskType == (int)EnumTaskType.CheckIn
                               && x.CreatedBy == empId && x.CreatedOn.Date >= startDate.Date && x.CreatedOn.Date <= endDate.Date).OrderBy(x => x.CreatedOn)
                         join key in _goalKeyRepo.GetQueryable() on task.GoalId equals key.GoalKeyId into tempKey
                         from goalKey in tempKey.DefaultIfEmpty()
                         select new TaskResponse
                         {
                             TaskId = task.TaskId,
                             GoalId = task.GoalId,
                             KeyDescription = goalKey.KeyDescription,
                             TaskName = task.TaskName,
                             IsCompleted = task.IsCompleted,
                             CreatedBy = task.CreatedBy,
                             CompletedDate = task.CompletedDate,
                             CreatedOn = task.CreatedOn,
                             TaskStartedDate=  task.TaskStartedDate.Date,
                             IsImported =task.IsImported
                         }).ToList();
            return tasks;
        }

        private bool IsCheckinVisible(long empId, long loggedInEmployeeId)
        {
            var isCheckinVisible = true;
            if (empId > 0 && empId != loggedInEmployeeId)
            {
                var checkinvisibality = CheckInVisibilty(empId);
                if (checkinvisibality.Item1 == (int)CheckInVisible.VisibleToReportingManager)
                {
                    var reportingUser = _employeeRepo.GetQueryable().FirstOrDefault(x =>
                    x.EmployeeId == empId);
                    var employeeCheckInVisble = _employeeCheckInVisiblePermissionsRepo.GetQueryable()
                        .FirstOrDefault(y => y.IsActive && y.CreatedBy == empId && y.EmployeeId
                        == loggedInEmployeeId);
                    if (reportingUser != null && reportingUser.ReportingTo
                        != loggedInEmployeeId && employeeCheckInVisble == null)
                        isCheckinVisible = false;
                }
            }
            return isCheckinVisible;
        }

        private Tuple<int, bool> CheckInVisibilty(long empId)
        {
            var teamsettings = _teamSettingRepo.GetQueryable().FirstOrDefault(x => x.IsActive);
            var checkinEmployeeMapping = _checkInEmployeeMappingRepo.GetQueryable().FirstOrDefault(x => x.EmployeeId == empId && x.IsActive);
            var isChangeCheckInVisibilty = teamsettings.IsChangeCheckInVisibilty;
            var checkInVisibilty = checkinEmployeeMapping == null ? teamsettings.CheckInVisibilty : checkinEmployeeMapping.CheckInVisibilty;
            return new Tuple<int, bool>(checkInVisibilty, isChangeCheckInVisibilty);
        }

        private string CheckInRemainingDays(int checkInDetailCount, int taskDetailsCount, double totalDays)
        {
            var msg = totalDays == 1 ? AppConstants.Day : AppConstants.Days;
            if (checkInDetailCount > 0 || taskDetailsCount > 0)
                return ResourceMessage.CheckInSubmitted;
            else if (totalDays >= 2)
                return ResourceMessage.CheckIn2daysAlert.Replace("X", Convert.ToString(totalDays, CultureInfo.CurrentCulture)).Replace("day", msg);
            else
                return ResourceMessage.CheckIn1dayAlert;
        }

        private async Task<List<CheckInWeeklyDatesResponse>> CalculateEmployeeWeeksAndStatus(long empId, int weekCount, bool isPlanningWeekVisible = false)
        {
            var checkIn = new List<CheckInWeeklyDatesResponse>();
            var empCreationDate = _employeeRepo.GetQueryable().FirstOrDefault(x => x.EmployeeId == empId);
            var checkInCreationDate = _checkInPointRepo.GetQueryable().FirstOrDefault();
            if (empCreationDate != null || checkInCreationDate != null)
            {
                checkIn = await GetWeeklyDateList(empCreationDate?.CreatedOn, checkInCreationDate?.CreatedOn, weekCount, isPlanningWeekVisible);

                checkIn.ForEach(x =>
                {
                    (x.CheckInStatus, x.CheckInStatusDetails) = GetCheckedInStatus(empId, x.StartDate, x.EndDate);
                });
            }
            return checkIn;
        }

        private async Task<List<CheckInWeeklyDatesResponse>> GetWeeklyDateList(DateTime? empCreationDate, DateTime? checkInCreationDate, int weekCount, bool isPlanningWeekVisible = false)
        {
            var endDate = await GetNextEndDate(isPlanningWeekVisible); //mar 4
            var startDate = endDate.AddDays(-6); //feb 26
            var dates = new List<CheckInWeeklyDatesResponse>();

            var lastWeekDate = await GetLastStartDate(empCreationDate > checkInCreationDate ? empCreationDate : checkInCreationDate);
            while (dates.Count < weekCount && startDate >= lastWeekDate)
            {
                dates.Add(
                    new CheckInWeeklyDatesResponse()
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        DisplayDate = startDate.ToString("MMM").ToUpper() + " " + startDate.Day + " - " + endDate.ToString("MMM").ToUpper() + " " + endDate.Day,
                        CheckInWeekType = GetCheckInWeekType(startDate, endDate)
                    });
                endDate = startDate.AddDays(-1);
                startDate = startDate.AddDays(-7);
            }

            return dates.OrderByDescending(x => x.EndDate).ToList();
        }

        private EnumCheckInWeekType GetCheckInWeekType(DateTime startDate, DateTime endDate)
        {
            if (endDate.Date < DateTime.UtcNow.Date)
                return EnumCheckInWeekType.AccomplishedWeek;
            else if (startDate.Date > DateTime.UtcNow.Date)
                return EnumCheckInWeekType.PlanningWeek;
            else
                return EnumCheckInWeekType.ExecutingWeek;
        }

        private async Task<DateTime> GetNextEndDate(bool isPlanningWeekVisible = false)
        {
            var nextEndDate = DateTime.UtcNow.Date;
            var checkInDetails = await GetConstantCheckInEndDateInDay();

            while (nextEndDate.DayOfWeek != (DayOfWeek)checkInDetails.Item1) //Firday  AppConstants.CheckInNextEndDate
                nextEndDate = nextEndDate.AddDays(1);

            //add future week
            if (isPlanningWeekVisible)
            {
                for (int i = 0; i < checkInDetails.Item2; i++)
                {
                    nextEndDate = nextEndDate.AddDays(7);
                }
            }

            return nextEndDate.Date;
        }

        private async Task<DateTime> GetLastStartDate(DateTime? date = null)
        {
            var lastWeekStartDate = date == null ? DateTime.UtcNow.Date : date.GetValueOrDefault().Date;
            var checkInEndDateInDay = (await GetConstantCheckInEndDateInDay()).Item1;
            if (checkInEndDateInDay == (int)DayOfWeek.Saturday)
                checkInEndDateInDay = (int)DayOfWeek.Sunday;
            else
                checkInEndDateInDay = checkInEndDateInDay + 1;
            while (lastWeekStartDate.DayOfWeek != (DayOfWeek)checkInEndDateInDay) //AppConstants.CheckInLastStartDate
                lastWeekStartDate = lastWeekStartDate.AddDays(-1);

            return lastWeekStartDate.Date;
        }

        private Tuple<EnumCheckInStatus, string> GetCheckedInStatus(long empId, DateTime startDate, DateTime endDate)
        {
            var weeklyEmpCheckin = _checkInDetailRepo.GetQueryable().Where(y => y.EmployeeId == empId && y.CheckInDate.Date >= startDate.Date && y.CheckInDate.Date <= endDate.Date).ToList();

            var taskDetails = _taskDetailRepo.GetQueryable().Where(x => x.IsActive && x.TaskType == (int)EnumTaskType.CheckIn
                               && x.CreatedBy == empId && x.CreatedOn.Date >= startDate.Date && x.CreatedOn.Date <= endDate.Date).ToList();

            EnumCheckInStatus checkInStatus;
            if (weeklyEmpCheckin.Count > 0 || taskDetails.Count > 0)
                checkInStatus = EnumCheckInStatus.CheckedIn;
            else if (endDate.Date >= DateTime.UtcNow.Date)
                checkInStatus = EnumCheckInStatus.NotYetCheckedIn;
            else
                checkInStatus = EnumCheckInStatus.CheckInMissed;

            return Tuple.Create(checkInStatus, EnumExtensionMethods.GetEnumDescription(checkInStatus));
        }

        private async Task<List<ObjectStatus>> GetFourCheckInStatus(long empId)
        {
            var objectStatus = new List<ObjectStatus>();
            var status = await CalculateEmployeeWeeksAndStatus(empId, AppConstants.FourWeek);
            if (status.ToList().Count > 0)
            {
                objectStatus = (from objStatus in status
                                select new ObjectStatus
                                {
                                    CheckInStatusId = (int)objStatus.CheckInStatus,
                                    CheckInStatus = objStatus.CheckInStatusDetails,
                                    DisplayDate = objStatus.StartDate.ToString("MMM") + " " + objStatus.StartDate.Day + " - " + objStatus.EndDate.ToString("MMM") + " " + objStatus.EndDate.Day,
                                    StartDate = objStatus.StartDate
                                }).ToList();
            }

            if (objectStatus.ToList().Count <= AppConstants.FourWeek)
            {
                int addDefaultValues = objectStatus.ToList().Count;
                for (int i = addDefaultValues; i < AppConstants.FourWeek; i++)
                {
                    var addObjectStatus = new ObjectStatus
                    {
                        CheckInStatusId = (int)EnumCheckInStatus.NoData,
                        CheckInStatus = EnumExtensionMethods.GetEnumDescription(EnumCheckInStatus.NoData)
                    };
                    objectStatus.Add(addObjectStatus);
                }
                objectStatus.Reverse();

            }

            return objectStatus;
        }

        private List<TaskDetail> GetNoCompletedPastTask(long empId, DateTime startDate, DateTime endDate)
        {
            var tasks = (from task in _taskDetailRepo.GetQueryable().Where(x => x.IsActive && x.TaskType == (int)EnumTaskType.CheckIn && !x.IsCompleted
                               && x.CreatedBy == empId && x.CreatedOn.Date >= startDate.Date && x.CreatedOn.Date <= endDate.Date).OrderBy(x => x.CreatedOn)
                         select task).ToList();
            return tasks;
        }
        #endregion
    }
}
