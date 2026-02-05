using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Services;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.Services
{
    public class NotificationsEmailsServiceTest
    {
        private readonly Mock<IServicesAggregator> mockIServicesAggregator;       
        private readonly Mock<IKeyVaultService> mockIKeyVaultService;
        private readonly Mock<ICommonService> mockICommonService;

        private readonly Mock<ILoggerFactory> mockILoggerFactory;        
        private readonly Mock<IRepositoryAsync<Employee>> mockEmployee;
        private readonly Mock<IRepositoryAsync<GoalObjective>> mockGoalObjective;
        private readonly Mock<IRepositoryAsync<GoalKey>> mockGoalKey;
        private readonly Mock<IRepositoryAsync<TeamCycleDetail>> mockTeamCycleDetail;
        private readonly Mock<IRepositoryAsync<CycleSymbol>> mockCycleSymbol;        
        private readonly Mock<UserIdentity> mockIUserIdentity;
        
        public NotificationsEmailsServiceTest()
        {
            mockIServicesAggregator = new Mock<IServicesAggregator>();            
            mockIKeyVaultService = new Mock<IKeyVaultService>();
            mockICommonService = new Mock<ICommonService>();
            mockILoggerFactory = new Mock<ILoggerFactory>();            
            mockEmployee = new Mock<IRepositoryAsync<Employee>>();
            mockGoalObjective = new Mock<IRepositoryAsync<GoalObjective>>();
            mockGoalKey = new Mock<IRepositoryAsync<GoalKey>>();
            mockTeamCycleDetail = new Mock<IRepositoryAsync<TeamCycleDetail>>();
            mockCycleSymbol = new Mock<IRepositoryAsync<CycleSymbol>>();            
            mockIUserIdentity = new Mock<UserIdentity>();
            

            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<Employee>()).Returns(mockEmployee.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<GoalObjective>()).Returns(mockGoalObjective.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<GoalKey>()).Returns(mockGoalKey.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<TeamCycleDetail>()).Returns(mockTeamCycleDetail.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<CycleSymbol>()).Returns(mockCycleSymbol.Object);
            mockIServicesAggregator.Setup(c => c.LoggerFactory).Returns(mockILoggerFactory.Object);
            mockICommonService.Setup(c => c.GetUserIdentity()).Returns(mockIUserIdentity.Object);
        }

        [Obsolete]
        public NotificationsEmailsService objNotificationsEmailsService()
        {
            return new NotificationsEmailsService(mockIServicesAggregator.Object, mockIKeyVaultService.Object, mockICommonService.Object);
        }

        #region NotificationsAndEmails
        [Fact]
        [Obsolete]
        public void UserNotificationsAndEmails_SendMail()
        {
            //Arrange  

            //Mock employee
            var employee = new List<Employee>()
            {
                new Employee(){
                    Designation = "test",
                    EmployeeId = 1,
                    FirstName ="test"
                },
                 new Employee(){
                    Designation = "test",
                    EmployeeId = 2,
                    FirstName ="test"
                }
            };
            //mock goalObjective table
            var goalObjective = new List<GoalObjective>()
            {
                new GoalObjective(){
                    GoalTypeId= 1,
                    GoalObjectiveId = 1
                }
            };
            //mock goalkey table
            var goalKey = new List<GoalKey>()
            {
                new GoalKey(){
                    GoalObjectiveId = 1,
                    GoalKeyId = 1
                }
            };
            // Mock KeyVault
            var mockBlobVaultResponse = new BlobVaultResponse()
            {
                BlobAccountName = "test",
                BlobAccountKey = "test"
            };
            var mockServiceSettingUrlResponse = new ServiceSettingUrlResponse()
            {
                FacebookUrl = "test"
            };
            // mock common service methods
            var mailerTemplateRequest = new MailerTemplateRequest()
            {
                IsActive = true,
                Subject = "test",
                Body = string.Empty
            };
            var teamCycleDetails = new List<TeamCycleDetail>
            {
                new TeamCycleDetail
                {
                    TeamCycleDetailId = 1,
                    CycleSymbolId = 1
                }                
            };
            var cycleSymbol = new List<CycleSymbol>
           {
                new CycleSymbol
                {
                    CycleSymbolId = 1,
                    CycleId = 1,
                    SymbolName = "test"
                }
            };

            var listLong = new List<long>() { 1,2 };

            var mock = employee.AsQueryable().BuildMock();
            mockEmployee.Setup(p => p.GetQueryable()).Returns(mock);
            mockEmployee.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(new Employee() { Designation = "test", EmployeeId = 1, FirstName = "test" });
                       
            var mockGoalObjectiveobj = goalObjective.AsQueryable().BuildMock();
            mockGoalObjective.Setup(p => p.GetQueryable()).Returns(mockGoalObjectiveobj);
            mockGoalObjective.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<GoalObjective, bool>>>())).ReturnsAsync(new GoalObjective() { GoalTypeId = 0 });
                        
            var mockGoalKeyobj = goalKey.AsQueryable().BuildMock();
            mockGoalKey.Setup(p => p.GetQueryable()).Returns(mockGoalKeyobj);
            mockGoalKey.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<GoalKey, bool>>>())).ReturnsAsync(new GoalKey() { GoalObjectiveId = 0, GoalStatusId = 1 });

            var mockTeamCycleDetails = teamCycleDetails.AsQueryable().BuildMock();
            mockTeamCycleDetail.Setup(p => p.GetQueryable()).Returns(mockTeamCycleDetails);
            mockTeamCycleDetail.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<TeamCycleDetail, bool>>>())).ReturnsAsync(new TeamCycleDetail() { TeamCycleDetailId = 1 });
            
            var cycleSymbolDetails = cycleSymbol.AsQueryable().BuildMock();
            mockCycleSymbol.Setup(p => p.GetQueryable()).Returns(cycleSymbolDetails);
            mockCycleSymbol.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CycleSymbol, bool>>>())).ReturnsAsync(new CycleSymbol() { CycleSymbolId = 1 , CycleId = 1 , SymbolName = "test"});



            mockIKeyVaultService.Setup(c => c.GetAzureBlobKeysAsync()).ReturnsAsync(mockBlobVaultResponse);
            mockIKeyVaultService.Setup(c => c.GetSettingsAndUrlsAsync()).ReturnsAsync(mockServiceSettingUrlResponse);
            
            mockICommonService.Setup(c => c.GetMailerTemplate(It.IsAny<string>())).ReturnsAsync(mailerTemplateRequest);
            mockICommonService.Setup(c => c.SendEmail(It.IsAny<MailRequest>(), It.IsAny<ServiceSettingUrlResponse>())).Verifiable();
            mockICommonService.Setup(c => c.Notifications(It.IsAny<NotificationsRequest>(), It.IsAny<ServiceSettingUrlResponse>())).Verifiable();

            var notificationsEmailsService = objNotificationsEmailsService();            

            //Act
            var result = notificationsEmailsService.UserNotificationsAndEmails(listLong, 1, 1, 1, 1, 1,"");

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        [Obsolete]
        public void UserNotificationsAndEmails_GoalTypeTwo()
        {
            //Arrange  

            //Mock employee
            var employee = new List<Employee>()
            {
                new Employee(){
                    Designation = "test",
                    EmployeeId = 1,
                    FirstName ="test"
                },
                 new Employee(){
                    Designation = "test",
                    EmployeeId = 2,
                    FirstName ="test"
                }
            };
            //mock goalObjective table
            var goalObjective = new List<GoalObjective>()
            {
                new GoalObjective(){
                    GoalTypeId= 2,
                    GoalObjectiveId = 1
                }
            };
            //mock goalkey table
            var goalKey = new List<GoalKey>()
            {
                new GoalKey(){
                    GoalObjectiveId = 1,
                    GoalKeyId = 1
                }
            };
            // Mock KeyVault
            var mockBlobVaultResponse = new BlobVaultResponse()
            {
                BlobAccountName = "test",
                BlobAccountKey = "test"
            };
            var mockServiceSettingUrlResponse = new ServiceSettingUrlResponse()
            {
                FacebookUrl = "test"
            };
            // mock common service methods
            var mailerTemplateRequest = new MailerTemplateRequest()
            {
                IsActive = true,
                Subject = "test",
                Body = string.Empty
            };
            var teamCycleDetails = new List<TeamCycleDetail>
            {
                new TeamCycleDetail
                {
                    TeamCycleDetailId = 1,
                    CycleSymbolId = 1
                }
            };
            var cycleSymbol = new List<CycleSymbol>
           {
                new CycleSymbol
                {
                    CycleSymbolId = 1,
                    CycleId = 1,
                    SymbolName = "test"
                }
            };

            var listLong = new List<long>() { 1, 2 };

            var mock = employee.AsQueryable().BuildMock();
            mockEmployee.Setup(p => p.GetQueryable()).Returns(mock);
            mockEmployee.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(new Employee() { Designation = "test", EmployeeId = 1, FirstName = "test" });

            var mockGoalObjectiveobj = goalObjective.AsQueryable().BuildMock();
            mockGoalObjective.Setup(p => p.GetQueryable()).Returns(mockGoalObjectiveobj);
            mockGoalObjective.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<GoalObjective, bool>>>())).ReturnsAsync(new GoalObjective() { GoalTypeId = 0 });

            var mockGoalKeyobj = goalKey.AsQueryable().BuildMock();
            mockGoalKey.Setup(p => p.GetQueryable()).Returns(mockGoalKeyobj);
            mockGoalKey.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<GoalKey, bool>>>())).ReturnsAsync(new GoalKey() { GoalObjectiveId = 0, GoalStatusId = 1 });

            var mockTeamCycleDetails = teamCycleDetails.AsQueryable().BuildMock();
            mockTeamCycleDetail.Setup(p => p.GetQueryable()).Returns(mockTeamCycleDetails);
            mockTeamCycleDetail.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<TeamCycleDetail, bool>>>())).ReturnsAsync(new TeamCycleDetail() { TeamCycleDetailId = 1 });

            var cycleSymbolDetails = cycleSymbol.AsQueryable().BuildMock();
            mockCycleSymbol.Setup(p => p.GetQueryable()).Returns(cycleSymbolDetails);
            mockCycleSymbol.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CycleSymbol, bool>>>())).ReturnsAsync(new CycleSymbol() { CycleSymbolId = 1, CycleId = 1, SymbolName = "test" });



            mockIKeyVaultService.Setup(c => c.GetAzureBlobKeysAsync()).ReturnsAsync(mockBlobVaultResponse);
            mockIKeyVaultService.Setup(c => c.GetSettingsAndUrlsAsync()).ReturnsAsync(mockServiceSettingUrlResponse);

            mockICommonService.Setup(c => c.GetMailerTemplate(It.IsAny<string>())).ReturnsAsync(mailerTemplateRequest);
            mockICommonService.Setup(c => c.SendEmail(It.IsAny<MailRequest>(), It.IsAny<ServiceSettingUrlResponse>())).Verifiable();
            mockICommonService.Setup(c => c.Notifications(It.IsAny<NotificationsRequest>(), It.IsAny<ServiceSettingUrlResponse>())).Verifiable();

            var notificationsEmailsService = objNotificationsEmailsService();

            //Act
            var result = notificationsEmailsService.UserNotificationsAndEmails(listLong, 1, 1, 2, 1, 1,"");

            //Assert
            Assert.NotNull(result);
        }
       
        #endregion
    }
}
