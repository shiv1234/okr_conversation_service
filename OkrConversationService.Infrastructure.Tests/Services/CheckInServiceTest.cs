using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using OkrConversationService.Application.Tests.MockData;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Infrastructure.Services;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.Services
{
    public class CheckInServiceTest
    {
        private readonly Mock<IServicesAggregator> _mockServiceAggregator;
        private readonly Mock<IRepositoryAsync<CheckInPoint>> _mockCheckInPointRepo;
        private readonly Mock<IRepositoryAsync<CheckInDetail>> _mockCheckInDetailRepo;
        private readonly Mock<IRepositoryAsync<Employee>> _mockEmployeeRepo;
        private readonly Mock<IRepositoryAsync<Constant>> _mockConstantRepo;
        private readonly Mock<CheckInGetAllQuery> _mockCheckInGetAllQuery;
        private readonly Mock<CheckInWeeklyDatesQuery> _mockCheckInWeeklyDatesQuery;
        private readonly Mock<AllDirectReportsEmployeeByIdQuery> _mockAllDirectReportsEmployeeByIdQuery;
        private readonly Mock<IRepositoryAsync<TeamSetting>> _mockTeamSettingRepo;
        private readonly Mock<IRepositoryAsync<CheckInEmployeeMapping>> _mockCheckInEmployeeMappingRepo;
        private readonly MockRepository _mockRepository;
        private readonly Mock<ICommonService> _mockCommonService;
        private readonly Mock<IRepositoryAsync<TaskDetail>> _mockTaskDetailRepo;
        private readonly Mock<IRepositoryAsync<GoalKey>> _mockGoalKeyRepo;
        private readonly Mock<CheckInDashboardQuery> _mockCheckInDashboardQuery;
        private readonly Mock<ImportPastTaskCommand> _mockImportPastTaskCommand;
        public CheckInServiceTest()
        {
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            _mockServiceAggregator = new Mock<IServicesAggregator>();
            var mockLogger = new Mock<ILogger<CheckInService>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));
            mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);
            _mockCommonService = new Mock<ICommonService>();
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockCheckInGetAllQuery = new Mock<CheckInGetAllQuery>();
            _mockCheckInWeeklyDatesQuery = new Mock<CheckInWeeklyDatesQuery>();
            _mockAllDirectReportsEmployeeByIdQuery = new Mock<AllDirectReportsEmployeeByIdQuery>();
            _mockCheckInPointRepo = new Mock<IRepositoryAsync<CheckInPoint>>();
            _mockConstantRepo = new Mock<IRepositoryAsync<Constant>>();
            _mockCheckInDetailRepo = new Mock<IRepositoryAsync<CheckInDetail>>();
            _mockEmployeeRepo = new Mock<IRepositoryAsync<Employee>>();
            _mockTeamSettingRepo = new Mock<IRepositoryAsync<TeamSetting>>();
            _mockCheckInEmployeeMappingRepo = new Mock<IRepositoryAsync<CheckInEmployeeMapping>>();
            _mockTaskDetailRepo = new Mock<IRepositoryAsync<TaskDetail>>();
            _mockGoalKeyRepo = new Mock<IRepositoryAsync<GoalKey>>();
            _mockCheckInDashboardQuery = new Mock<CheckInDashboardQuery>();
            _mockImportPastTaskCommand = new Mock<ImportPastTaskCommand>();
            _mockServiceAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<CheckInPoint>()).Returns(_mockCheckInPointRepo.Object);
            _mockServiceAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<CheckInDetail>()).Returns(_mockCheckInDetailRepo.Object);
            _mockServiceAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<Employee>()).Returns(_mockEmployeeRepo.Object);
            _mockServiceAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<Constant>()).Returns(_mockConstantRepo.Object);
            _mockServiceAggregator.Setup(c => c.LoggerFactory).Returns(mockLoggerFactory.Object);
            _mockServiceAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<TeamSetting>()).Returns(_mockTeamSettingRepo.Object);
            _mockServiceAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<CheckInEmployeeMapping>()).Returns(_mockCheckInEmployeeMappingRepo.Object);
            _mockServiceAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<TaskDetail>()).Returns(_mockTaskDetailRepo.Object);
            _mockServiceAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<GoalKey>()).Returns(_mockGoalKeyRepo.Object);

        }

        [Obsolete("")]
        public CheckInService CheckInService()
        {
            return new CheckInService(_mockServiceAggregator.Object, _mockCommonService.Object);
        }

        #region GetAll
        [Fact]
        [Obsolete("")]
        public async Task GetAll_PassValidation_Successfull()
        {
            //arrange   
            var mockcheckInPoints = MockCheckInService.MockDbCheckInPoints().AsQueryable().BuildMock();
            var mockcheckInDetails = MockCheckInService.MockDbCheckInDetails().AsQueryable().BuildMock();
            var mockConstants = MockCheckInService.MockDbConstants().AsQueryable().BuildMock();
            var mockTaskDetails = new List<TaskDetail>().AsQueryable().BuildMock();

            //SetUp
            UserIdentity userIdentity = new UserIdentity()
            {
                EmployeeId = 1
            };
            _mockCheckInPointRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInPoints);
            _mockCheckInDetailRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInDetails);
            _mockCommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);
            _mockConstantRepo.Setup(x => x.GetQueryable()).Returns(mockConstants);
            _mockTaskDetailRepo.Setup(x => x.GetQueryable()).Returns(mockTaskDetails);

            //act
            var result = await CheckInService().GetAll(_mockCheckInGetAllQuery.Object);

            //Assert            
            Assert.True(result.IsSuccess);
            _mockRepository.VerifyAll();

        }

        #endregion

        #region GetAllCheckInWeekly
        [Fact]
        [Obsolete("")]
        public async Task GetAllCheckInWeeklyDates_Successfull()
        {
            //arrange   
            var mockCrossEmployees = MockCheckInService.MockDbCrossEmployees().AsQueryable().BuildMock();
            var mockcheckInPoints = MockCheckInService.MockDbCheckInPoints().AsQueryable().BuildMock();
            var mockcheckInDetails = new List<CheckInDetail>().AsQueryable().BuildMock();
            var mockConstants = MockCheckInService.MockDbConstants().AsQueryable().BuildMock();
            //SetUp
            UserIdentity userIdentity = new UserIdentity()
            {
                EmployeeId = 1
            };

            _mockEmployeeRepo.Setup(x => x.GetQueryable()).Returns(mockCrossEmployees);
            _mockCheckInPointRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInPoints);
            _mockCheckInDetailRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInDetails);
            _mockCommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);
            _mockConstantRepo.Setup(x => x.GetQueryable()).Returns(mockConstants);
            //act
            var result = await CheckInService().GetAllCheckInWeeklyDates(_mockCheckInWeeklyDatesQuery.Object);

            //Assert            
            Assert.True(result.IsSuccess);
            _mockRepository.VerifyAll();

        }

        [Fact]
        [Obsolete("")]
        public async Task GetAllCheckInWeeklyDates_SuccessfullNoRecordFound()
        {
            //arrange   
            var mockCrossEmployees = new List<Employee>().AsQueryable().BuildMock();
            var mockcheckInPoints = new List<CheckInPoint>().AsQueryable().BuildMock();
            var mockcheckInDetails = new List<CheckInDetail>().AsQueryable().BuildMock();
            //SetUp
            UserIdentity userIdentity = new UserIdentity()
            {
                EmployeeId = 1
            };

            _mockEmployeeRepo.Setup(x => x.GetQueryable()).Returns(mockCrossEmployees);
            _mockCheckInPointRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInPoints);
            _mockCheckInDetailRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInDetails);
            _mockCommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);

            //act
            var result = await CheckInService().GetAllCheckInWeeklyDates(_mockCheckInWeeklyDatesQuery.Object);

            //Assert            
            Assert.True(result.IsSuccess);
            Assert.Equal(result.Status, (int)HttpStatusCode.NoContent);
            Assert.True(result.Entity == null);
            Assert.Equal(result.MessageList["message"], ResourceMessage.RecordNotFoundMessage);
            _mockRepository.VerifyAll();

        }

        #endregion

        #region GetAllDirectReportsByIds
        //[Fact]
        //[Obsolete("")]
        //public async Task GetAllDirectReportsByIds_Successfull()
        //{
        //    //arrange   
        //    var mockCrossEmployees = MockCheckInService.MockDbCrossEmployees().AsQueryable().BuildMock();
        //    var mockcheckInPoints = MockCheckInService.MockDbCheckInPoints().AsQueryable().BuildMock();
        //    var mockcheckInDetails = new List<CheckInDetail>().AsQueryable().BuildMock();
        //    var mockConstants = MockCheckInService.MockDbConstants().AsQueryable().BuildMock();
        //    //SetUp
        //    UserIdentity userIdentity = new UserIdentity()
        //    {
        //        EmployeeId = 1
        //    };

        //    _mockEmployeeRepo.Setup(x => x.GetQueryable()).Returns(mockCrossEmployees);
        //    _mockCommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);
        //    _mockCheckInPointRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInPoints);
        //    _mockCheckInDetailRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInDetails);
        //    _mockConstantRepo.Setup(x => x.GetQueryable()).Returns(mockConstants);
        //    //act
        //    var result = await CheckInService().GetAllDirectReportsByIds(_mockAllDirectReportsEmployeeByIdQuery.Object);

        //    //Assert            
        //    Assert.True(result.IsSuccess);

        //    _mockRepository.VerifyAll();

        //}
        #endregion

        #region IsCheckInSubmitted
        [Fact]
        [Obsolete("")]
        public async Task IsCheckInSubmitted_Successfull()
        {
            var mockCrossEmployees = MockCheckInService.MockDbCrossEmployees().AsQueryable().BuildMock();
            var mockcheckInPoints = MockCheckInService.MockDbCheckInPoints().AsQueryable().BuildMock();
            var mockcheckInDetails = new List<CheckInDetail>().AsQueryable().BuildMock();
            var mockConstants = MockCheckInService.MockDbConstants().AsQueryable().BuildMock();
            var mockTeamSetting = MockCheckInService.MockDbTeamSetting().AsQueryable().BuildMock();
            var mockCheckInEmployeeMapping = MockCheckInService.MockCheckInEmployeeMapping().AsQueryable().BuildMock();
            var mockGoalKey = MockCheckInService.MockGoalKey().AsQueryable().BuildMock();
            var mockTaskDetails = new List<TaskDetail>().AsQueryable().BuildMock();
            //SetUp
            var userIdentity = new UserIdentity()
            {
                EmployeeId = 1
            };

            _mockEmployeeRepo.Setup(x => x.GetQueryable()).Returns(mockCrossEmployees);
            _mockCommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);
            _mockCheckInPointRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInPoints);
            _mockCheckInDetailRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInDetails);
            _mockConstantRepo.Setup(x => x.GetQueryable()).Returns(mockConstants);
            _mockTeamSettingRepo.Setup(x => x.GetQueryable()).Returns(mockTeamSetting);
            _mockCheckInEmployeeMappingRepo.Setup(x => x.GetQueryable()).Returns(mockCheckInEmployeeMapping);
            _mockGoalKeyRepo.Setup(x => x.GetQueryable()).Returns(mockGoalKey);
            _mockTaskDetailRepo.Setup(x => x.GetQueryable()).Returns(mockTaskDetails);
            //act
            var result = await CheckInService().IsCheckInSubmitted();

            //Assert            
            Assert.True(result.IsSuccess);

            _mockRepository.VerifyAll();

        }
        #endregion

        #region UpdateCheckinVisibility
        [Fact]
        [Obsolete("")]
        public async Task UpdateCheckinVisibilityWithEmployeeIdExist_Successfull()
        {
            //SetUp
            var mockCheckInEmployeeMapping = MockCheckInService.MockCheckInEmployeeMapping().AsQueryable().BuildMock();
            var mockTeamSetting = new List<TeamSetting>() { new TeamSetting() { CheckInVisibilty = 1, IsChangeCheckInVisibilty = true, IsActive = true, } }.AsQueryable().BuildMock();
            var userIdentity = new UserIdentity()
            {
                EmployeeId = 1
            };
            _mockCommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);
            _mockTeamSettingRepo.Setup(x => x.GetQueryable()).Returns(mockTeamSetting);
            _mockCheckInEmployeeMappingRepo.Setup(x => x.GetQueryable()).Returns(mockCheckInEmployeeMapping);

            UpdateCheckinVisibilityCommand cmd = new UpdateCheckinVisibilityCommand()
            {
                IsActive = true,
                CreatedOn = DateTime.Now,
                CheckInVisibilty = (CheckInVisible)1,
            };

            //act
            var result = await CheckInService().UpdateCheckinVisibility(cmd);

            //Assert            
            Assert.True(result.IsSuccess);
            _mockRepository.VerifyAll();

        }
        [Fact]
        [Obsolete("")]
        public async Task UpdateCheckinVisibilityWithEmployeeIdNotExist_Successfull()
        {
            //SetUp
            var mockCheckInEmployeeMapping = MockCheckInService.MockCheckInEmployeeMapping().AsQueryable().BuildMock();
            var mockTeamSetting = new List<TeamSetting>() { new TeamSetting() { CheckInVisibilty = 1, IsChangeCheckInVisibilty = true, IsActive = true, } }.AsQueryable().BuildMock();
            var userIdentity = new UserIdentity()
            {
                EmployeeId = -22
            };
            _mockCommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);
            _mockTeamSettingRepo.Setup(x => x.GetQueryable()).Returns(mockTeamSetting);
            _mockCheckInEmployeeMappingRepo.Setup(x => x.GetQueryable()).Returns(mockCheckInEmployeeMapping);

            UpdateCheckinVisibilityCommand cmd = new UpdateCheckinVisibilityCommand()
            {
                IsActive = true,
                CreatedOn = DateTime.Now,
                CheckInVisibilty = (CheckInVisible)1,
            };

            //act
            var result = await CheckInService().UpdateCheckinVisibility(cmd);

            //Assert            
            Assert.True(result.IsSuccess);
            _mockRepository.VerifyAll();

        }

        [Fact]
        [Obsolete("")]
        public async Task UpdateCheckinVisibility_Failled()
        {
            //SetUp
            var mockCheckInEmployeeMapping = MockCheckInService.MockCheckInEmployeeMapping().AsQueryable().BuildMock();
            var mockTeamSetting = new List<TeamSetting>() { new TeamSetting() { CheckInVisibilty = 1, IsChangeCheckInVisibilty = false, IsActive = true, } }.AsQueryable().BuildMock();
            var userIdentity = new UserIdentity()
            {
                EmployeeId = 1
            };
            _mockCommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);
            _mockTeamSettingRepo.Setup(x => x.GetQueryable()).Returns(mockTeamSetting);
            _mockCheckInEmployeeMappingRepo.Setup(x => x.GetQueryable()).Returns(mockCheckInEmployeeMapping);

            UpdateCheckinVisibilityCommand cmd = new UpdateCheckinVisibilityCommand()
            {
                IsActive = true,
                CreatedOn = DateTime.Now,
                CheckInVisibilty = (CheckInVisible)1,
            };

            //act
            var result = await CheckInService().UpdateCheckinVisibility(cmd);

            //Assert            
            Assert.False(result.IsSuccess);
            _mockRepository.VerifyAll();

        }
        #endregion

        #region DashboardCheckIn
        [Fact]
        [Obsolete("")]
        public async Task DashboardCheckIn_Successfull()
        {
            //arrange   
            var mockcheckInPoints = MockCheckInService.MockDbCheckInPoints().AsQueryable().BuildMock();
            var mockcheckInDetails = MockCheckInService.MockDbCheckInDetails().AsQueryable().BuildMock();
            var mockConstants = MockCheckInService.MockDbConstants().AsQueryable().BuildMock();
            var mockTaskDetails = new List<TaskDetail>().AsQueryable().BuildMock();

            //SetUp
            UserIdentity userIdentity = new UserIdentity()
            {
                EmployeeId = 1
            };
            _mockCheckInPointRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInPoints);
            _mockCheckInDetailRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInDetails);
            _mockCommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);
            _mockConstantRepo.Setup(x => x.GetQueryable()).Returns(mockConstants);
            _mockTaskDetailRepo.Setup(x => x.GetQueryable()).Returns(mockTaskDetails);

            //act
            var result = await CheckInService().GetDashboardCheckInDetails(
                _mockCheckInDashboardQuery.Object);
            //var result = await CheckInService().GetAll(_mockCheckInGetAllQuery.Object);

            //Assert            
            Assert.True(result.IsSuccess);
            _mockRepository.VerifyAll();

        }

        #endregion
        #region Import
        [Fact]
        [Obsolete("")]
        public async Task Import_Successfull()
        {
            //arrange   
            var mockcheckInPoints = MockCheckInService.MockDbCheckInPoints().AsQueryable().BuildMock();
            var mockcheckInDetails = MockCheckInService.MockDbCheckInDetails().AsQueryable().BuildMock();
            var mockConstants = MockCheckInService.MockDbConstants().AsQueryable().BuildMock();
            var mockTaskDetails = new List<TaskDetail>().AsQueryable().BuildMock();

            //SetUp
            UserIdentity userIdentity = new UserIdentity()
            {
                EmployeeId = 1,
                IsImpersonatedUser=true
            };
            _mockCheckInPointRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInPoints);
            _mockCheckInDetailRepo.Setup(x => x.GetQueryable()).Returns(mockcheckInDetails);
            _mockCommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);
            _mockConstantRepo.Setup(x => x.GetQueryable()).Returns(mockConstants);
            _mockTaskDetailRepo.Setup(x => x.GetQueryable()).Returns(mockTaskDetails);

            //act
            var result = await CheckInService().ImportPastTask(
                _mockImportPastTaskCommand.Object);
            //var result = await CheckInService().GetAll(_mockCheckInGetAllQuery.Object);

            //Assert            
            Assert.True(result.IsSuccess);
            _mockRepository.VerifyAll();

        }

        #endregion
    }
}
