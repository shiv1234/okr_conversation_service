using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;

namespace OkrConversationService.Application.Tests.MockData
{
    public static class MockCheckInService
    {
        public static Payload<CheckInPointsResponse> MockGetAllResponse()
        {
            return new Payload<CheckInPointsResponse>()
            {
                Entity =
                        new CheckInPointsResponse()
                        {
                            CheckInDetailsResponse = new List<CheckInDetailsResponse>() { new CheckInDetailsResponse {  CheckInPointsId = 1,
                               CheckInPoints = "Admin",
                               CheckInDetailsId = 1,
                               CheckInDetails = "Done"}
                           },
                            TaskResponse = new List<TaskResponse>() { new TaskResponse { TaskId =1,
                           GoalId=0,
                           TaskName="TaskName",
                           IsCompleted=true,
                           CreatedBy=1,
                           IsImported=false,
                           TaskStartedDate =DateTime.UtcNow
                           } }
                        },
                IsSuccess = true
            };
        }
        public static Payload<CheckInPointsResponse> MockGetNoRecordResponse()
        {
            return new Payload<CheckInPointsResponse>()
            {
                MessageList = new Dictionary<string, string>() { { "message", ResourceMessage.RecordNotFoundMessage } },


                IsSuccess = true
            };
        }
        public static Payload<CheckInDetailRequest> MockCheckInCreateRequest()
        {
            return new Payload<CheckInDetailRequest>()
            {
                Entity = new CheckInDetailRequest
                {
                    CheckInPointsId = 1,
                    EmployeeId = 123,
                    CheckInDetailsId = 1,
                    CheckInDetails = "Done"
                }
            };
        }
        public static Payload<CheckInDatesPermissionResponse> MockGetAllCheckInWeeklyResponse()
        {
            return new Payload<CheckInDatesPermissionResponse>()
            {
                Entity = new CheckInDatesPermissionResponse()
                {
                    CheckInWeeklyDatesResponse = new List<CheckInWeeklyDatesResponse>() {
                        new CheckInWeeklyDatesResponse()
                        {
                            StartDate = DateTime.Now.Date,
                            EndDate = DateTime.Now.Date,
                            DisplayDate = DateTime.Now.Date.ToString(),
                            CheckInStatus = EnumCheckInStatus.CheckedIn,
                            CheckInStatusDetails = "Done"
                        }},
                    IsCheckinDataVisible = true
                },
                IsSuccess = true
            };
        }

        public static Payload<DirectReportsResponse> MockGetAllDirectReportsByIdsResponse()
        {
            var objectStatus = new List<ObjectStatus> { new ObjectStatus
            {
                     CheckInStatusId=1,
                     CheckInStatus="NoData"
             }};
            return new Payload<DirectReportsResponse>()
            {
                Entity =
                        new DirectReportsResponse()
                        {
                            EmployeeId = 1,
                            FirstName = "ABC",
                            LastName = "Last",
                            Designation = "Engineer",
                            ImagePath = null,
                            CheckInStatus = objectStatus
                        },
                IsSuccess = true
            };
        }

        public static Payload<DirectreportsResponseResult> MockGetAllDirectReportsByIdsResponseResult()
        {
            var objectStatus = new List<ObjectStatus> { new ObjectStatus
            {
                     CheckInStatusId=1,
                     CheckInStatus="NoData"
             }};

            Payload<DirectreportsResponseResult> responseResult = new Payload<DirectreportsResponseResult>();
            List<DirectReportsResponse> directResult = new List<DirectReportsResponse>();
            directResult.Add(
                 new DirectReportsResponse()
                 {
                     EmployeeId = 1,
                     FirstName = "ABC",
                     LastName = "Last",
                     Designation = "Engineer",
                     ImagePath = null,
                     CheckInStatus = objectStatus
                 });
            responseResult.Entity = new DirectreportsResponseResult { DirectReports = directResult };


            return responseResult;
        }



        public static Payload<CheckInAlertResponse> MockIsCheckInSubmittedResponse()
        {
            return new Payload<CheckInAlertResponse>()
            {
                Entity =
                        new CheckInAlertResponse()
                        {
                            IsAlert = true,
                            RemainingDays = "5"
                        },
                IsSuccess = true
            };
        }

        public static List<CheckInPoint> MockDbCheckInPoints()
        {
            List<CheckInPoint> checkInPoints = new List<CheckInPoint>();
            checkInPoints.Add(new CheckInPoint() { CheckInPointsId = 1, IsActive = true, CreatedOn = DateTime.Now });
            checkInPoints.Add(new CheckInPoint() { CheckInPointsId = 2, IsActive = true, CreatedOn = DateTime.Now });
            return checkInPoints;

        }

        public static List<CheckInDetail> MockDbCheckInDetails()
        {
            List<CheckInDetail> CheckInDetails = new List<CheckInDetail>();
            CheckInDetails.Add(new CheckInDetail() { CheckInDetailsId = 1, CheckInPointsId = 1, IsActive = true });
            CheckInDetails.Add(new CheckInDetail() { CheckInDetailsId = 2, CheckInPointsId = 2, IsActive = true });
            return CheckInDetails;

        }
        public static List<Constant> MockDbConstants()
        {
            List<Constant> Constants = new List<Constant>();
            Constants.Add(new Constant { ConstantId = 1, ConstantName = "CheckInEndDateInDay", ConstantValue = "5", IsActive = true, CreatedBy = 1, CreatedOn = DateTime.Now, });
            Constants.Add(new Constant { ConstantId = 2, ConstantName = "CheckInFutureWeekDisplayCount", ConstantValue = "1", IsActive = true, CreatedBy = 1, CreatedOn = DateTime.Now, });

            return Constants;

        }


        public static List<Employee> MockDbCrossEmployees()
        {
            List<Employee> crossEmployees = new List<Employee>();
            crossEmployees.Add(new Employee() { EmployeeId = 1, IsActive = true, CreatedOn = DateTime.Now, ReportingTo = 1 });
            crossEmployees.Add(new Employee() { EmployeeId = 2, IsActive = true, CreatedOn = DateTime.Now, ReportingTo = 1 });
            return crossEmployees;

        }

        public static List<TeamSetting> MockDbTeamSetting()
        {
            List<TeamSetting> teamSetting = new List<TeamSetting>();
            teamSetting.Add(new TeamSetting() { TeamSettingId = 1, IsActive = true, CreatedOn = DateTime.Now, CheckInVisibilty = 1, IsChangeCheckInVisibilty = false });
            teamSetting.Add(new TeamSetting() { TeamSettingId = 2, IsActive = true, CreatedOn = DateTime.Now, CheckInVisibilty = 1, IsChangeCheckInVisibilty = false });
            return teamSetting;

        }

        public static List<CheckInEmployeeMapping> MockCheckInEmployeeMapping()
        {
            List<CheckInEmployeeMapping> checkInEmployeeMapping = new List<CheckInEmployeeMapping>();
            checkInEmployeeMapping.Add(new CheckInEmployeeMapping() { CheckInEmployeeMappingId = 1, IsActive = true, CreatedOn = DateTime.Now, CheckInVisibilty = 1, EmployeeId = 1 });
            checkInEmployeeMapping.Add(new CheckInEmployeeMapping() { CheckInEmployeeMappingId = 2, IsActive = true, CreatedOn = DateTime.Now, CheckInVisibilty = 1, EmployeeId = 2 });
            return checkInEmployeeMapping;

        }
        public static List<GoalKey> MockGoalKey()
        {
            return new List<GoalKey>
            {
                new GoalKey() { GoalKeyId = 1, IsActive = true, CreatedOn = DateTime.Now, KeyDescription = "KeyDescription", EmployeeId = 1 },
                new GoalKey() { GoalKeyId = 2, IsActive = true, CreatedOn = DateTime.Now, KeyDescription = "KeyDescription 2", EmployeeId = 2 }
            };
        }
        public static Payload<DashboardCheckInResponse> MockGetAllDashboardCheckInResponse()
        {
            var objectStatus = new List<ObjectStatus> { new ObjectStatus
            {
                     CheckInStatusId=1,
                     CheckInStatus="NoData"
             }};
            return new Payload<DashboardCheckInResponse>()
            {
                Entity =
                        new DashboardCheckInResponse()
                        {
                            TaskCount = 1,
                            DisplayDate = DateTime.UtcNow.ToLongDateString(),
                            IsAlert = true,
                            IsCheckInSubmitted = true,
                            RemaingDaysLeft = 1,
                            RemainingDays = "",

                            CheckInStatus = objectStatus
                        },
                IsSuccess = true
            };
        }
    }
}
