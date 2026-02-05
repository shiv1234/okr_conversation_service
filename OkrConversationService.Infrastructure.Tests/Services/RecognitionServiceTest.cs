using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Infrastructure.Services;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using System.Threading.Tasks;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;

namespace OkrConversationService.Infrastructure.Tests.Services
{
    public class RecognitionServiceTest
    {
        private readonly Mock<IServicesAggregator> mockIServicesAggregator;
        private readonly Mock<IKeyVaultService> mockIKeyVaultService;
        private readonly Mock<ICommonService> mockICommonService;
        private readonly Mock<INotificationsEmailsService> mockINotificationService;
        private readonly Mock<ISystemService> mockISystemService;
        private readonly Mock<ILoggerFactory> mockILoggerFactory;
        private readonly Mock<IRepositoryAsync<Employee>> mockEmployee;
        private readonly Mock<IRepositoryAsync<GoalObjective>> mockGoalObjective;
        private readonly Mock<IRepositoryAsync<GoalKey>> mockGoalKey;
        private readonly Mock<IRepositoryAsync<TeamCycleDetail>> mockTeamCycleDetail;
        private readonly Mock<IRepositoryAsync<CycleSymbol>> mockCycleSymbol;
        private readonly Mock<IRepositoryAsync<Conversation>> mockConversation;
        private readonly Mock<IRepositoryAsync<LikeReaction>> mockConversationReaction;
        private readonly Mock<IRepositoryAsync<ConversationFile>> mockConversationFile;
        private readonly Mock<IRepositoryAsync<ConversationLog>> mockConversationLog;
        private readonly Mock<IRepositoryAsync<CommentDetails>> mockCommentDetails;
        private readonly Mock<IRepositoryAsync<RecognitionCategory>> mockRecognitionCategory;
        private readonly Mock<IRepositoryAsync<Recognition>> mockRecognition;
        private readonly Mock<IRepositoryAsync<RecognitionImageMapping>> mockRecognitionImageMapping;
        private readonly Mock<IRepositoryAsync<Team>> mockTeam;
        private readonly Mock<IRepositoryAsync<NotificationsDetail>> mockNotificationsDetail;
        private readonly Mock<IRepositoryAsync<EmployeeTeamMapping>> mockEmployeeTeamMapping;
        private readonly Mock<IRepositoryAsync<EmployeeTag>> mockEmployeeTag;
        private readonly Mock<IRepositoryAsync<EmployeeEngagement>> mockEmployeeEngagement;
        private readonly Mock<IRepositoryAsync<RecognitionEmployeeTeamMapping>> mockRecognitionEmployeeTeamMapping;
        private readonly Mock<UserIdentity> mockIUserIdentity;
        public RecognitionServiceTest()
        {
            mockIServicesAggregator = new Mock<IServicesAggregator>();
            mockINotificationService = new Mock<INotificationsEmailsService>();
            mockIKeyVaultService = new Mock<IKeyVaultService>();
            mockICommonService = new Mock<ICommonService>();
            mockISystemService = new Mock<ISystemService>();
            mockILoggerFactory = new Mock<ILoggerFactory>();
            mockEmployee = new Mock<IRepositoryAsync<Employee>>();
            mockGoalObjective = new Mock<IRepositoryAsync<GoalObjective>>();
            mockGoalKey = new Mock<IRepositoryAsync<GoalKey>>();
            mockTeamCycleDetail = new Mock<IRepositoryAsync<TeamCycleDetail>>();
            mockCycleSymbol = new Mock<IRepositoryAsync<CycleSymbol>>();
            mockConversation = new Mock<IRepositoryAsync<Conversation>>();
            mockConversationReaction = new Mock<IRepositoryAsync<LikeReaction>>();
            mockCommentDetails = new Mock<IRepositoryAsync<CommentDetails>>();
            mockRecognitionCategory = new Mock<IRepositoryAsync<RecognitionCategory>>();
            mockRecognition = new Mock<IRepositoryAsync<Recognition>>();
            mockConversationFile = new Mock<IRepositoryAsync<ConversationFile>>();
            mockConversationLog = new Mock<IRepositoryAsync<ConversationLog>>();
            mockIUserIdentity = new Mock<UserIdentity>();
            mockRecognitionImageMapping = new Mock<IRepositoryAsync<RecognitionImageMapping>>();
            mockEmployeeTeamMapping = new Mock<IRepositoryAsync<EmployeeTeamMapping>>();
            mockNotificationsDetail = new Mock<IRepositoryAsync<NotificationsDetail>>();
            mockRecognitionEmployeeTeamMapping = new Mock<IRepositoryAsync<RecognitionEmployeeTeamMapping>>();
            mockEmployeeEngagement = new Mock<IRepositoryAsync<EmployeeEngagement>>();
            mockTeam = new Mock<IRepositoryAsync<Team>>();
            mockEmployeeTag = new Mock<IRepositoryAsync<EmployeeTag>>();
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<RecognitionEmployeeTeamMapping>()).Returns(mockRecognitionEmployeeTeamMapping.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<Employee>()).Returns(mockEmployee.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<GoalObjective>()).Returns(mockGoalObjective.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<GoalKey>()).Returns(mockGoalKey.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<TeamCycleDetail>()).Returns(mockTeamCycleDetail.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<CycleSymbol>()).Returns(mockCycleSymbol.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<Conversation>()).Returns(mockConversation.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<LikeReaction>()).Returns(mockConversationReaction.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<ConversationLog>()).Returns(mockConversationLog.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<CommentDetails>())
                .Returns(mockCommentDetails.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<EmployeeEngagement>()).Returns(mockEmployeeEngagement.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<ConversationFile>()).Returns(mockConversationFile.Object);
            mockIServicesAggregator.Setup(c => c.LoggerFactory).Returns(mockILoggerFactory.Object);
            mockICommonService.Setup(c => c.GetUserIdentity()).Returns(mockIUserIdentity.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<RecognitionCategory>())
               .Returns(mockRecognitionCategory.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<Recognition>())
               .Returns(mockRecognition.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<RecognitionImageMapping>())
               .Returns(mockRecognitionImageMapping.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<Team>())
             .Returns(mockTeam.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<EmployeeTeamMapping>())
             .Returns(mockEmployeeTeamMapping.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<NotificationsDetail>())
            .Returns(mockNotificationsDetail.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<EmployeeTag>())
         .Returns(mockEmployeeTag.Object);
        }
        [Obsolete]
        public RecognitionService ObjRecognitionService()
        {
            var CommentDetails = new List<CommentDetails>
            {
                new CommentDetails{
                    CreatedBy = 1,
                    ModuleDetailsId=1,
                    CommentDetailsId=1,
                    ModuleId=1,
                    Comments="test",
                    IsActive = true,
                }
            };
            var mock = CommentDetails.AsQueryable().BuildMock();
            mockCommentDetails.Setup(x => x.GetQueryable()).Returns(mock);
            mockCommentDetails.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<CommentDetails,
                bool>>>())).ReturnsAsync(new CommentDetails()
                {
                    IsActive = true,
                    CommentDetailsId = 1,
                    Comments = "test"
                ,
                    ModuleDetailsId = 1,
                    ModuleId = 1
                });

            var recognitionImageMappings = new List<RecognitionImageMapping>
            {
                new RecognitionImageMapping
                {
                     IsActive=true,
                     FileName="",
                     GuidFileName="",
                     Name="",
                     RecognitionCategoryId=1,
                     RecognitionCategoryTypeId=1,
                      RecognitionId=1,
                      RecognitionImageMappingId=1

                }
            };
            var mockReco = recognitionImageMappings.AsQueryable().BuildMock();
            mockRecognitionImageMapping.Setup(x => x.GetQueryable()).Returns(mockReco);
            mockRecognitionImageMapping.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<RecognitionImageMapping, bool>>>()))
                .ReturnsAsync(new RecognitionImageMapping()
                {
                    RecognitionId = 1,
                    IsActive = true,
                    RecognitionCategoryId = 1,
                    RecognitionCategoryTypeId = 1,
                    RecognitionImageMappingId = 1,
                    Name = "",
                    FileName = "",
                    GuidFileName = ""
                });

            var recognitionEmployeeTeamMapping = new List<RecognitionEmployeeTeamMapping>
            {
                new RecognitionEmployeeTeamMapping
                {RecognitionEmployeeTeamMappingId = 1,
                    TeamId = 1,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = 1,
                    EmployeeId = 1,
                    IsActive = true,
                    IsGivenByManager = false,
                    RecognitionId = 1

                }
            };
            var mockRecoEmp = recognitionEmployeeTeamMapping.AsQueryable().BuildMock();
            mockRecognitionEmployeeTeamMapping.Setup(x => x.GetQueryable()).Returns(mockRecoEmp);
            mockRecognitionEmployeeTeamMapping.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<RecognitionEmployeeTeamMapping, bool>>>()))
                .ReturnsAsync(new RecognitionEmployeeTeamMapping()
                {
                    RecognitionEmployeeTeamMappingId = 1,
                    TeamId = 1,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = 1,
                    EmployeeId = 1,
                    IsActive = true,
                    IsGivenByManager = false,
                    RecognitionId = 1

                });

            var recognitionCategory = new List<RecognitionCategory>
            {
                new RecognitionCategory{
                    IsActive=true,
                    RecognitionCategoryTypeId=1,
                    RecognitionCategoryId=1,
                    Name="",
                    FileName="",
                    IsDefault=true,
                    IsOnlyManager=false,
                    GuidFileName=""
                }
            };
            var mockCat = recognitionCategory.AsQueryable().BuildMock();
            mockRecognitionCategory.Setup(x => x.GetQueryable()).Returns(mockCat);
            mockRecognitionCategory.Setup(r => r.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<RecognitionCategory, bool>>>()))
                .ReturnsAsync(new RecognitionCategory()
                {
                    IsActive = true,
                    GuidFileName = "",
                    FileName = "",
                    IsDefault = true,
                    IsOnlyManager = true,
                    Name = "",
                    RecognitionCategoryId = 1,
                    RecognitionCategoryTypeId = 1
                });
            var employee = new List<Employee>()
            {
                new Employee(){
                    Designation = "test",
                    EmployeeId = 1,
                    FirstName ="test",
                    ImagePath = "",
                    IsActive=true
                },
                 new Employee(){
                    Designation = "test",
                    EmployeeId = 2,
                    FirstName ="test",
                    IsActive=true
                }
            };
            var mockEmp = employee.AsQueryable().BuildMock();
            mockEmployee.Setup(p => p.GetQueryable()).Returns(mockEmp);
            mockEmployee.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(new Employee() { Designation = "test", EmployeeId = 1, FirstName = "test" });
            var team = new List<Team>()
            {
                new Team(){
                   IsActive=true,
                   CreatedBy=1,
                   CreatedOn=DateTime.UtcNow,
                   BackGroundColorCode="",
                   Colorcode="",
                   TeamId=1,
                   ParentId=0,
                   TeamName="",
                   Description="",
                   LogoImagePath="",
                   LogoName="",
                   TeamHead=0,
                },
                 new Team(){
                   IsActive=true,
                   CreatedBy=1,
                   CreatedOn=DateTime.UtcNow,
                   BackGroundColorCode="",
                   Colorcode="",
                   TeamId=2,
                   ParentId=0,
                   TeamName="",
                   Description="",
                   LogoImagePath="",
                   LogoName="",
                   TeamHead=0,
                }
            };
            var mockTeamBuild = team.AsQueryable().BuildMock();
            mockTeam.Setup(p => p.GetQueryable()).Returns(mockTeamBuild);
            mockTeam.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Team, bool>>>()))
                .ReturnsAsync(new Team()
                {
                    IsActive = true,
                    TeamHead = 0,
                    LogoName = "",
                    ParentId = 1,
                    LogoImagePath = "",
                    TeamId = 1
                });
            var recognition = new List<Recognition>
            {
                new Recognition{
                    IsActive=true,
                    ReceiverId=1,
                    RecognitionId=1,
                    RecognitionCategoryTypeId=1,
                    IsGivenByManager=true,
                    IsAttachment=true,
                    Headlines="",
                    Message="",
                    CreatedBy=1,
                    CreatedOn=DateTime.UtcNow
                }
            };
            var mockRecog = recognition.AsQueryable().BuildMock();
            mockRecognition.Setup(x => x.GetQueryable()).Returns(mockRecog);
            mockRecognition.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Recognition, bool>>>()))
                .ReturnsAsync(new Recognition()
                {
                    ReceiverId = 1,
                    RecognitionCategoryTypeId = 1,
                    IsActive = true,
                    IsAttachment = true,
                    IsGivenByManager = true,
                    RecognitionId = 1,
                    Message = "test",
                    CreatedOn=DateTime.MinValue,
                    CreatedBy=1,
                    Headlines="",
                    UpdatedBy=1,
                    UpdatedOn=DateTime.MaxValue
                });


            var notificationsDetail = new List<NotificationsDetail>
            {
                new NotificationsDetail{
                    CreatedOn=DateTime.UtcNow,
                    Actionable=true,
                    ApplicationMasterId=1,
                    IsDeleted=true,
                    IsRead=true,
                    MessageTypeId=1,
                    NotificationOnId=1,
                    NotificationOnTypeId=1,
                    NotificationsBy=1,
                    NotificationsDetailsId=1,
                    NotificationsMessage="",
                    NotificationsTo=1,
                    NotificationTypeId=1,

                }
            };
            var mockNotifications = notificationsDetail.AsQueryable().BuildMock();
            mockNotificationsDetail.Setup(x => x.GetQueryable()).Returns(mockNotifications);
            mockNotificationsDetail.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<NotificationsDetail, bool>>>()))
                .ReturnsAsync(new NotificationsDetail()
                {
                    CreatedOn = DateTime.UtcNow,
                    Actionable = true,
                    ApplicationMasterId = 1,
                    IsDeleted = true,
                    IsRead = true,
                    MessageTypeId = 1,
                    NotificationOnId = 1,
                    NotificationOnTypeId = 1,
                    NotificationsBy = 1,
                    NotificationsDetailsId = 1,
                    NotificationsMessage = "",
                    NotificationsTo = 1,
                    NotificationTypeId = 1,
                });


            var employeeTeamMapping = new List<EmployeeTeamMapping>
            {
                new EmployeeTeamMapping{
                    IsActive=true,
                    TeamId=1,
                    EmployeeId=1,
                    EmployeeTeamMappingId=1,
                    CreatedBy=1,
                    CreatedOn=DateTime.UtcNow
                }
            };
            var mockEmployeeTeam = employeeTeamMapping.AsQueryable().BuildMock();
            mockEmployeeTeamMapping.Setup(x => x.GetQueryable()).Returns(mockEmployeeTeam);

            var blobVaultResponse = new BlobVaultResponse
            {
                BlobAccountKey = "",
                BlobAccountName = "",
                BlobCdnCommonUrl = "",
                BlobCdnUrl = "",
                BlobContainerName = ""
            };
            mockIKeyVaultService.Setup(x => x.GetAzureBlobKeysAsync())
                .ReturnsAsync(blobVaultResponse);

            var likeReaction = new List<LikeReaction>
            {
                new LikeReaction{
                    ModuleDetailsId = 1,
                    IsActive = true ,
                    ModuleId = 2,
                    EmployeeId = 1
                }
            };
            var mockConReaction = likeReaction.AsQueryable().BuildMock();
            mockConversationReaction.Setup(x => x.GetQueryable()).Returns(mockConReaction);


            var employeeTags = new List<EmployeeTag>
            {
                new EmployeeTag{
                    IsActive=true,
                    EmployeeTagId=1,
                    TagId=1,
                    ModuleDetailsId=1,
                    CreatedBy=1,
                    CreatedOn=DateTime.UtcNow.Date,
                    ModuleId=7
                },
                new EmployeeTag{
                    IsActive=true,
                    EmployeeTagId=1,
                    TagId=1,
                    ModuleDetailsId=1,
                    CreatedBy=1,
                    CreatedOn=DateTime.UtcNow.Date,
                    ModuleId=6
                }
            };
            var mockEmployeetags = employeeTags.AsQueryable().BuildMock();
            mockEmployeeTag.Setup(x => x.GetQueryable()).Returns(mockEmployeetags);


            var employeeEngagements = new List<EmployeeEngagement>
            {
                new EmployeeEngagement{
                    IsActive=true,
                    EmployeeId=1,
                    TeamId=1,
                    EmployeeEngagementId=1,
                    EngagementTypeId=21,
                    TeamHeadId=1,
                    UpdatedOn=DateTime.UtcNow,
                    CreatedOn=DateTime.UtcNow
                }
            };
            var mockEmployeeEngagements = employeeEngagements.AsQueryable().BuildMock();
            mockEmployeeEngagement.Setup(x => x.GetQueryable()).Returns(mockEmployeeEngagements);
            return new RecognitionService(mockIServicesAggregator.Object,
                mockINotificationService.Object, mockIKeyVaultService.Object,
                mockICommonService.Object, mockISystemService.Object);
        }
        #region GetRecognitionLike
        [Fact]
        [Obsolete]
        public void GetRecognitionLike_RecordFound()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var recognitionLikeQueryRequest = new RecognitionLikeQuery
            {
                ModuleDetailsId = 1
            };
            //Act
            var result = recognitionService.GetRecognitionLike(recognitionLikeQueryRequest);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        [Obsolete]
        public void GetRecognitionLike_IsSuccessTrue()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();

            var recognitionLikeQueryRequest = new RecognitionLikeQuery
            {
                ModuleDetailsId = 1
            };
            var userIdentity = new UserIdentity
            {
                EmployeeId = 1
            };

            mockICommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);
            //Act
            var result = recognitionService.GetRecognitionLike(recognitionLikeQueryRequest);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.Result.IsSuccess);
        }



        #endregion
        #region GetComments
        [Fact]
        [Obsolete]
        public async Task GetComments_NoRecordFound()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var getCommentQuery = new GetCommentQuery
            {
                ModuleDetailsId = 1,
                PageIndex = 1,
                PageSize = 10
            };
            //Act
            var result = await recognitionService.GetComments(getCommentQuery);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        [Obsolete]
        public async Task GetComments_IsSuccessTrue()
        {
            var recognitionService = ObjRecognitionService();
            var getCommentQuery = new GetCommentQuery
            {
                ModuleDetailsId = 1,
                PageIndex = 1,
                PageSize = 10
            };
            var result = await recognitionService.GetComments(getCommentQuery);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
        #endregion
        #region GetCategory
        [Fact]
        [Obsolete]
        public async Task GetCategory_NoRecordFound()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var recognitionCategoryGetQuery = new RecognitionCategoryGetQuery
            {
                EmployeeId = 1,
            };
            //Act
            var result = await recognitionService.GetCategory(recognitionCategoryGetQuery);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        [Obsolete]
        public async Task GetCategory_IsSuccessTrue()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var recognitionCategoryGetQuery = new RecognitionCategoryGetQuery
            {
                EmployeeId = 1,
            };
            //Act
            var result = await recognitionService.GetCategory(recognitionCategoryGetQuery);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
        #endregion
        #region GetOrgRecognition
        [Fact]
        [Obsolete]
        public async Task GetOrgRecognition_NoRecordFound()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var getOrgRecognitionQuery = new GetOrgRecognitionQuery
            {
                Id = 1,
                IsMyPost = true,                
            };
            var result = await recognitionService.GetOrgRecognition(getOrgRecognitionQuery);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        [Obsolete]
        public async Task GetOrgRecognition_IsSuccessTrue()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var getOrgRecognitionQuery = new GetOrgRecognitionQuery
            {
                Id = 1,
                IsMyPost = true               
            };
            //Act
            var result = await recognitionService.GetOrgRecognition(getOrgRecognitionQuery);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
        #endregion
        #region GetMyWallOfFame
        [Fact]
        [Obsolete]
        public async Task GetMyWallOfFame_NoRecordFound()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var query = new MyWallOfFameGetQuery()
            {
                MyMyWallOfFameRequest
                = new Domain.RequestModel.MyWallOfFameRequest { Id = 1, PageIndex = 1, PageSize = 100}
            };

            //Act
            var result = await recognitionService.GetMyWallOfFameGetQuery(query);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        [Obsolete]
        public async Task GetMyWallOfFame_SearchType_Zero()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var query = new MyWallOfFameGetQuery()
            {
                MyMyWallOfFameRequest
               = new Domain.RequestModel.MyWallOfFameRequest { 
                   Id = 1, SearchType = 0, PageIndex = 1,
                   PageSize = 100
               }
            };
            //Act
            var result = await recognitionService.GetMyWallOfFameGetQuery(query);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
        [Fact]
        [Obsolete]
        public async Task GetMyWallOfFame_SearchType_One()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var query = new MyWallOfFameGetQuery()
            {
                MyMyWallOfFameRequest
               = new Domain.RequestModel.MyWallOfFameRequest { Id = 1,
                   SearchType = 1, PageIndex = 1, PageSize = 100}
            };
            //Act
            var result = await recognitionService.GetMyWallOfFameGetQuery(query);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Obsolete]
        public async Task GetMyWallOfFame_SearchType_Two()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var query = new MyWallOfFameGetQuery()
            {
                MyMyWallOfFameRequest
               = new Domain.RequestModel.MyWallOfFameRequest { Id = 1, SearchType = 2,
                   PageIndex = 1, PageSize = 100}
            };
            //Act
            var result = await recognitionService.GetMyWallOfFameGetQuery(query);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
        #endregion
        #region GetTeamsByEmpId
        [Fact]
        [Obsolete]
        public async Task GetTeamsByEmpId_NoRecordFound()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var teamsByEmpIdGetQuery = new TeamsByEmpIdGetQuery
            {
                EmployeeId = 1
            };
            //Act
            var result = await recognitionService.GetTeamsByEmpId(teamsByEmpIdGetQuery);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        [Obsolete]
        public async Task GetTeamsByEmpId_IsSuccessTrue()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var teamsByEmpIdGetQuery = new TeamsByEmpIdGetQuery
            {
                EmployeeId = 1
            };
            //Act
            var result = await recognitionService.GetTeamsByEmpId(teamsByEmpIdGetQuery);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
        #endregion
        #region MyWallOfFameDashBoard
        [Fact]
        [Obsolete]
        public async Task MyWallOfFameDashBoard_NoRecordFound()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var myWallOfFameDashBoardGetQuery = new MyWallOfFameDashBoardGetQuery
            { };
            //Act
            var result = await recognitionService.MyWallOfFameDashBoard(myWallOfFameDashBoardGetQuery);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        [Obsolete]
        public async Task MyWallOfFameDashBoard_IsSuccessTrue()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var myWallOfFameDashBoardGetQuery = new MyWallOfFameDashBoardGetQuery
            { };
            //Act
            var result = await recognitionService.MyWallOfFameDashBoard(myWallOfFameDashBoardGetQuery);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
        #endregion
        #region RecognitionByTeamId
        //[Fact]
        //[Obsolete]
        //public async Task RecognitionByTeamId_NoRecordFound()
        //{
        //    //Arrange
        //    var recognitionService = ObjRecognitionService();
        //    var recognitionByTeamIdGetQuery = new RecognitionByTeamIdGetQuery
        //    { Team = new RecognitionByTeamIdRequest { TeamId = 1, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow } };
        //    //Act
        //    var result = await recognitionService.RecognitionByTeamId(recognitionByTeamIdGetQuery);
        //    //Assert
        //    Assert.NotNull(result);
        //}
        //[Fact]
        //[Obsolete]
        //public async Task RecognitionByTeamId_IsSuccessTrue()
        //{
        //    //Arrange
        //    var recognitionService = ObjRecognitionService();
        //    var recognitionByTeamIdGetQuery = new RecognitionByTeamIdGetQuery
        //    { Team = new RecognitionByTeamIdRequest { TeamId = 1, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow } };
        //    //Act
        //    var result = await recognitionService.RecognitionByTeamId(recognitionByTeamIdGetQuery);
        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.True(result.IsSuccess);
        //}
        #endregion
        #region CreateComment
        //[Fact]
        //[Obsolete]
        //public void CreateComment_IsSuccess()
        //{
        //    //Arrange
        //    var commentCreateCommand = new CommentCreateCommand
        //    {
        //        IsActive = true,
        //        CreatedOn = DateTime.Today,

        //        CommentDetailsRequest = new CommentDetailsRequest
        //        {
        //            CommentDetailsId = 1,
        //            Comments = "test",
        //            ModuleDetailsId = 1,
        //            RecognitionImageRequests = new List<RecognitionImageRequest> {
        //                new RecognitionImageRequest { FileName="", GuidFileName="" } }
        //        },
        //        UpdatedOn = DateTime.Today
        //    };
        //    var identity = new UserIdentity
        //    {
        //        EmployeeId = 1
        //    };
        //    mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
        //    var payload = new Payload<bool> { Status = 200, Entity = true, IsSuccess = false, };

        //    mockICommonService.Setup(p => p.IsBlockedWords(commentCreateCommand.CommentDetailsRequest.Comments)).ReturnsAsync(payload);

        //    IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
        //    mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
        //    var recognitionService = ObjRecognitionService();
        //    //Act
        //    var result = recognitionService.CreateComments(commentCreateCommand);
        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(200, result.Result.Status);
        //    Assert.True(result.Result.IsSuccess);
        //}
        //[Fact]
        //[Obsolete]
        //public void CreateComment_CommentDetailsId_Zero_IsSuccess()
        //{
        //    //Arrange
        //    var commentCreateCommand = new CommentCreateCommand
        //    {
        //        IsActive = true,
        //        CreatedOn = DateTime.Today,
        //        CommentDetailsRequest = new CommentDetailsRequest
        //        {
        //            CommentDetailsId = 0,
        //            Comments = "test",
        //            ModuleDetailsId = 1,
        //            RecognitionImageRequests = new List<RecognitionImageRequest> {
        //                new RecognitionImageRequest { FileName="", GuidFileName="" } }
        //        },
        //        UpdatedOn = DateTime.Today
        //    };
        //    var identity = new UserIdentity
        //    {
        //        EmployeeId = 1
        //    };
        //    mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
        //    IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
        //    var payload = new Payload<bool> { Status = 200, Entity = true, IsSuccess = false, };

        //    mockICommonService.Setup(p => p.IsBlockedWords(commentCreateCommand.CommentDetailsRequest.Comments)).ReturnsAsync(payload);

        //    mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
        //    var recognitionService = ObjRecognitionService();
        //    //Act
        //    var result = recognitionService.CreateComments(commentCreateCommand);
        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(200, result.Result.Status);
        //    Assert.True(result.Result.IsSuccess);
        //}

        //[Fact]
        //[Obsolete]
        //public void CreateComment_GuidFileName_IsSuccess()
        //{
        //    //Arrange
        //    var commentCreateCommand = new CommentCreateCommand
        //    {
        //        IsActive = true,
        //        CreatedOn = DateTime.Today,
        //        CommentDetailsRequest = new CommentDetailsRequest
        //        {
        //            CommentDetailsId = 0,
        //            Comments = "test",
        //            ModuleDetailsId = 1,
        //            RecognitionImageRequests = new List<RecognitionImageRequest> {
        //                new RecognitionImageRequest { FileName="", GuidFileName="abc.jpg" } }
        //        },

        //        UpdatedOn = DateTime.Today
        //    };
        //    var identity = new UserIdentity
        //    {
        //        EmployeeId = 1
        //    };
        //    mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
        //    IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
        //    var payload = new Payload<bool> { Status = 200, Entity = true, IsSuccess = false, };

        //    mockICommonService.Setup(p => p.IsBlockedWords(commentCreateCommand.CommentDetailsRequest.Comments)).ReturnsAsync(payload);

        //    mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
        //    var recognitionService = ObjRecognitionService();

        //    var recognitionImageMappings = new List<RecognitionImageMapping>
        //    {
        //        new RecognitionImageMapping
        //        {
        //             IsActive=true,
        //             FileName="",
        //             GuidFileName="abc.jpg",
        //             Name="",
        //             RecognitionCategoryId=1,
        //             RecognitionCategoryTypeId=(int)RecognitionCategoryType.RecognitionCommentScreenshot,
        //              RecognitionId=1,
        //              RecognitionImageMappingId=1,



        //        }
        //    };
        //    var mockReco = recognitionImageMappings.AsQueryable().BuildMock();
        //    mockRecognitionImageMapping.Setup(x => x.GetQueryable()).Returns(mockReco);
        //    mockRecognitionImageMapping.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<RecognitionImageMapping, bool>>>()))
        //        .ReturnsAsync(new RecognitionImageMapping()
        //        {
        //            RecognitionId = 1,
        //            IsActive = true,
        //            RecognitionCategoryId = 1,
        //            RecognitionCategoryTypeId = 1,
        //            RecognitionImageMappingId = 1,
        //            Name = "",
        //            FileName = "",
        //            GuidFileName = ""
        //        });


        //    //Act
        //    var result = recognitionService.CreateComments(commentCreateCommand);
        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(200, result.Result.Status);
        //    Assert.True(result.Result.IsSuccess);
        //}
        #endregion
        #region DeleteComment
        [Fact]
        [Obsolete]
        public void DeleteComment_IsSuccess()
        {
            //Arrange
            var commentDeleteCommand = new CommentDeleteCommand
            {
                IsActive = true,
                CreatedOn = DateTime.Today,
                UpdatedOn = DateTime.Today,
                CommentDetailsId = 1,

            };
            var identity = new UserIdentity
            {
                EmployeeId = 1
            };
            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
            IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
            var recognitionService = ObjRecognitionService();
            //Act
            var result = recognitionService.DeleteComment(commentDeleteCommand);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
            Assert.True(result.Result.IsSuccess);
        }
        #endregion
        #region Edit

        #endregion
        //#region Delete
        //[Fact]
        //[Obsolete]
        //public void Delete_IsSuccess()
        //{
        //    //Arrange
        //    var commentDeleteCommand = new RecognitionDeleteCommand
        //    {
        //        IsActive = true,
        //        CreatedOn = DateTime.Today,
        //        UpdatedOn = DateTime.Today,
        //        RecognitionId = 1,


        //    };
        //    var identity = new UserIdentity
        //    {
        //        EmployeeId = 1
        //    };
        //    mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
        //    IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
        //    mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
        //    var recognitionService = ObjRecognitionService();
        //    //Act
        //    var result = recognitionService.Delete(commentDeleteCommand);
        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(200, result.Result.Status);
        //    Assert.True(result.Result.IsSuccess);
        //}
        //#endregion

        #region EmployeesLeaderBoard
        [Fact]
        [Obsolete]
        public async Task EmployeesLeaderBoard_SerachType_All()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var query = new EmployeesLeaderBoardGetQuery()
            {
                Request
               = new Domain.RequestModel.EmployeesLeaderBoardRequest
               {
                   Id = 1,
                   SearchType = 0
                 
               }
            };
            //Act
            var result = await recognitionService.EmployeesLeaderBoard(query);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Obsolete]
        public async Task EmployeesLeaderBoard_SerachType_Employee()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var query = new EmployeesLeaderBoardGetQuery()
            {
                Request
               = new Domain.RequestModel.EmployeesLeaderBoardRequest
               {
                   Id = 1,
                   SearchType = 1
               }
            };
            //Act
            var result = await recognitionService.EmployeesLeaderBoard(query);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Obsolete]
        public async Task EmployeesLeaderBoard_SerachType_Team()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var query = new EmployeesLeaderBoardGetQuery()
            {
                Request
               = new Domain.RequestModel.EmployeesLeaderBoardRequest
               {
                   Id = 1,
                   SearchType = 2                 
               }
            };
            //Act
            var result = await recognitionService.EmployeesLeaderBoard(query);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
        #endregion

        #region TeamsLeaderBoard
        [Fact]
        [Obsolete]
        public async Task TeamsLeaderBoard_SerachType_All()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var query = new TeamsLeaderBoardGetQuery()
            {
                Request
               = new Domain.RequestModel.RecognitionLeaderBoardRequest
               {
                   Id = 1,
                   SearchType = 0                  
               }
            };
            //Act
            var result = await recognitionService.TeamsLeaderBoard(query);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Obsolete]
        public async Task TeamsLeaderBoard_SerachType_Employee()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var query = new TeamsLeaderBoardGetQuery()
            {
                Request
               = new Domain.RequestModel.RecognitionLeaderBoardRequest
               {
                   Id = 1,
                   SearchType = 1,                  
               }
            };
            //Act
            var result = await recognitionService.TeamsLeaderBoard(query);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        [Obsolete]
        public async Task TeamsLeaderBoard_SerachType_Team()
        {
            //Arrange
            var recognitionService = ObjRecognitionService();
            var query = new TeamsLeaderBoardGetQuery()
            {
                Request
               = new Domain.RequestModel.RecognitionLeaderBoardRequest
               {
                   Id = 1,
                   SearchType = 2                  
               }
            };
            //Act
            var result = await recognitionService.TeamsLeaderBoard(query);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
        #endregion

    }
}