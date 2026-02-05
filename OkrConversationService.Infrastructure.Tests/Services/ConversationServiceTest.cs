using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.Commands;
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
using OkrConversationService.Domain.ResponseModels;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace OkrConversationService.Infrastructure.Tests.Services
{
    public class ConversationServiceTest
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
        private readonly Mock<IRepositoryAsync<EmployeeTag>> mockConversationEmployeeTag;
        private readonly Mock<UserIdentity> mockIUserIdentity;

        public ConversationServiceTest()
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
            mockConversationEmployeeTag = new Mock<IRepositoryAsync<EmployeeTag>>();
            mockConversationFile = new Mock<IRepositoryAsync<ConversationFile>>();
            mockConversationLog = new Mock<IRepositoryAsync<ConversationLog>>();
            mockIUserIdentity = new Mock<UserIdentity>();


            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<Employee>()).Returns(mockEmployee.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<GoalObjective>()).Returns(mockGoalObjective.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<GoalKey>()).Returns(mockGoalKey.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<TeamCycleDetail>()).Returns(mockTeamCycleDetail.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<CycleSymbol>()).Returns(mockCycleSymbol.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<Conversation>()).Returns(mockConversation.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<LikeReaction>()).Returns(mockConversationReaction.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<ConversationLog>()).Returns(mockConversationLog.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<EmployeeTag>()).Returns(mockConversationEmployeeTag.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<ConversationFile>()).Returns(mockConversationFile.Object);
            mockIServicesAggregator.Setup(c => c.LoggerFactory).Returns(mockILoggerFactory.Object);
            mockICommonService.Setup(c => c.GetUserIdentity()).Returns(mockIUserIdentity.Object);
        }

        [Obsolete]
        public ConversationService ObjConversationService()
        {
            return new ConversationService(mockIServicesAggregator.Object, mockINotificationService.Object, mockIKeyVaultService.Object, mockICommonService.Object , mockISystemService.Object);
        }

        #region GetAll
        [Fact]
        [Obsolete]
        public void GetAll_NoRecordFound()
        {
            //Arrange
            var conversationService = ObjConversationService();
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = -1,
                    ConversationId = 1,
                    Description = "Conversation",
                    IsActive = true
                }
            };
            var conversationGetAllQueryRequest = new ConversationGetAllQuery
            {
                GoalTypeId = 1,
                GoalSourceId = 1
            };
            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

            //Act
            var result = conversationService.GetAll(conversationGetAllQueryRequest);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        [Obsolete]
        public void GetAll_IsSuccessTrue()
        {
            //Arrange
            var conversationService = ObjConversationService();
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = -1,
                    ConversationId = 1,
                    GoalSourceId = 1,
                    GoalTypeId = 1,
                    Description = "Conversation",
                    IsActive = true
                }
            };
            var conversationReaction = new List<LikeReaction>
            {
                new LikeReaction{
                    ModuleDetailsId = 1,
                    IsActive = true ,
                    ModuleId = 1
                }
            };

            var conversationLog = new List<ConversationLog>
            {
                new ConversationLog
                {
                    EmployeeId = 1
                }
            };

            var conversationGetAllQueryRequest = new ConversationGetAllQuery
            {
                GoalTypeId = 1,
                GoalSourceId = 1,
                PageIndex = 10,
                PageSize = 1
            };
            var userIdentity = new UserIdentity
            {
                EmployeeId = 1
            };

            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

            var mockConReaction = conversationReaction.AsQueryable().BuildMock();
            mockConversationReaction.Setup(x => x.GetQueryable()).Returns(mockConReaction);

            var mockConLog = conversationLog.AsQueryable().BuildMock();
            mockConversationLog.Setup(x => x.GetQueryable()).Returns(mockConLog);


            mockICommonService.Setup(x => x.GetUserIdentity()).Returns(userIdentity);

            var ConversationService = ObjConversationService();

            //Act
            var result = ConversationService.GetAll(conversationGetAllQueryRequest);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Result.IsSuccess);
        }
        #endregion

        #region Create
        [Fact]
        [Obsolete]
        public void Create_IsSuccess()
        {
            //Arrange
            var conversationCreateCommand = new ConversationCreateCommand
            {
                IsActive = true,
                CreatedOn = DateTime.Today,
                ConversationCreateRequest = new ConversationCreateRequest
                {
                    Description = "test",
                    assignedFiles = new List<ConversationFiles> { new ConversationFiles { FileName = ""} },
                    employeeTags = new List<ConversationEmployeeTags> { new ConversationEmployeeTags { EmployeeId = 1} }
                },
                UpdatedOn = DateTime.Today
            };
            var identity = new UserIdentity
            {
                EmployeeId = 1
            };

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);

            IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);

            var ConversationService = ObjConversationService();

            //Act
            var result = ConversationService.Create(conversationCreateCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
            Assert.True(result.Result.IsSuccess);
        }
        #endregion

        #region Edit
        [Fact]
        [Obsolete]
        public void Edit_NoRecordFound()
        {
            //Arrange
            var conversationEditCommand = new ConversationEditCommand
            {
                IsActive = true,
                CreatedOn = DateTime.Today,
                ConversationEditRequest = new ConversationEditRequest
                {
                    Description = "test",
                    assignedFiles = new List<ConversationFiles> { new ConversationFiles { FileName = "" } },
                    employeeTags = new List<ConversationEmployeeTags> { new ConversationEmployeeTags { EmployeeId = 1 } }
                },
                UpdatedOn = DateTime.Today
                
            };
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = -1,
                    ConversationId = 1,
                    Description = "Conversation",
                    IsActive = true
                }
            };
            var identity = new UserIdentity
            {
                EmployeeId = 1
            };
            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
            IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
            var ConversationService = ObjConversationService();

            //Act
            var result = ConversationService.Edit(conversationEditCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.Result.Status);
            Assert.False(result.Result.IsSuccess);
        }

        [Fact]
        [Obsolete]
        public void Edit_SomeThingWentRong()
        {
            //Arrange
            var conversationEditCommand = new ConversationEditCommand
            {
                IsActive = true,
                CreatedOn = DateTime.Today,
                ConversationEditRequest = new ConversationEditRequest
                {
                    Description = "test",
                    assignedFiles = new List<ConversationFiles> { new ConversationFiles { FileName = "" } },
                    employeeTags = new List<ConversationEmployeeTags> { new ConversationEmployeeTags { EmployeeId = 1 } },
                    ConversationId = 1
                },
                UpdatedOn = DateTime.Today

            };
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = -1,
                    ConversationId = 1,
                    Description = "Conversation",
                    IsActive = true
                }
            };
            var identity = new UserIdentity
            {
                EmployeeId = 1
            };
            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);
            mockConversation.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).ReturnsAsync(new Conversation() { GoalTypeId = 0 , ConversationId = 1 });

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
            IOperationStatus opStatus = new OperationStatus { Success = false, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
            var ConversationService = ObjConversationService();

            //Act
            var result = ConversationService.Edit(conversationEditCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.Result.Status);
            Assert.False(result.Result.IsSuccess);
        }

        [Fact]
        [Obsolete]
        public void Edit_SuccessConversationFileExists()
        {
            //Arrange
            var conversationEditCommand = new ConversationEditCommand
            {
                
                IsActive = true,
                CreatedOn = DateTime.Today,
                ConversationEditRequest = new ConversationEditRequest
                {
                    IsActive = true,
                    Description = "test",
                    assignedFiles = new List<ConversationFiles> { new ConversationFiles { FileName = "test1",StorageFileName = "test1" } },
                    employeeTags = new List<ConversationEmployeeTags> { new ConversationEmployeeTags { EmployeeId = 1} },
                    ConversationId = 1,
                    Type = 1
                },
                UpdatedOn = DateTime.Today

            };
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = -1,
                    ConversationId = 1,
                    Description = "Conversation",
                    IsActive = true
                }
            };
            var conversationFile = new List<ConversationFile>
            {
                new ConversationFile{
                    CreatedBy = -1,
                    ConversationId = 1,
                    FileName = "Conversation1",
                    StorageFileName = "test1",
                    IsActive = true,
                    ConversationFileId = 1
                },
                 new ConversationFile{
                    CreatedBy = -1,
                    ConversationId = 2,
                    FileName = "Conversation2",
                    StorageFileName = "test2",
                    IsActive = true,
                    ConversationFileId = 2
                }
            };
            var conversationEmpTag = new List<EmployeeTag>
            {
                new EmployeeTag{
                    CreatedBy = -1,
                    ModuleDetailsId = 1,
                    TagId = 1,
                    EmployeeTagId = 1,
                    IsActive = true                    
                }
            };
            var identity = new UserIdentity
            {
                EmployeeId = 1
            };
            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

            var mockConfile = conversationFile.AsQueryable().BuildMock();
            mockConversationFile.Setup(x => x.GetQueryable()).Returns(mockConfile);

            var mockConEmpTag = conversationEmpTag.AsQueryable().BuildMock();
            mockConversationEmployeeTag.Setup(x => x.GetQueryable()).Returns(mockConEmpTag);

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
            mockConversation.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).ReturnsAsync(new Conversation { ConversationId = 1 });
            IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
            var ConversationService = ObjConversationService();

            //Act
            var result = ConversationService.Edit(conversationEditCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
        }

        [Fact]
        [Obsolete]
        public void Edit_SuccessConversationFileNotExists()
        {
            //Arrange
            var conversationEditCommand = new ConversationEditCommand
            {
                IsActive = true,
                CreatedOn = DateTime.Today,
                ConversationEditRequest = new ConversationEditRequest
                {
                    Description = "test",
                    assignedFiles = new List<ConversationFiles> { new ConversationFiles { FileName = "test1", StorageFileName = "test1" } },
                    employeeTags = new List<ConversationEmployeeTags> { new ConversationEmployeeTags { EmployeeId = 1 } },
                    ConversationId = 2
                },
                UpdatedOn = DateTime.Today

            };
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = -1,
                    ConversationId = 1,
                    Description = "Conversation",
                    IsActive = true
                }
            };
            var conversationFile = new List<ConversationFile>
            {
                new ConversationFile{
                    CreatedBy = -1,
                    ConversationId = 2,
                    FileName = "Conversation1",
                    StorageFileName = "test1",
                    IsActive = true,
                    ConversationFileId = 1
                }
                // new ConversationFile{
                //    CreatedBy = -1,
                //    ConversationId = 2,
                //    FileName = "Conversation2",
                //    StorageFileName = "test2",
                //    IsActive = true,
                //    ConversationFileId = 2
                //}
            };
            var conversationEmpTag = new List<EmployeeTag>
            {
                new EmployeeTag{
                    CreatedBy = -1,
                    ModuleDetailsId = 1,
                    TagId = 1,
                    EmployeeTagId = 1,
                    IsActive = true
                }
            };
            var identity = new UserIdentity
            {
                EmployeeId = 1
            };
            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

            var mockConfile = conversationFile.AsQueryable().BuildMock();
            mockConversationFile.Setup(x => x.GetQueryable()).Returns(mockConfile);

            var mockConEmpTag = conversationEmpTag.AsQueryable().BuildMock();
            mockConversationEmployeeTag.Setup(x => x.GetQueryable()).Returns(mockConEmpTag);

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
            mockConversation.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).ReturnsAsync(new Conversation { ConversationId = 1 });
            IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
            var ConversationService = ObjConversationService();

            //Act
            var result = ConversationService.Edit(conversationEditCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
        }

        [Fact]
        [Obsolete]
        public void Edit_SuccessConversationOldFileExists()
        {
            //Arrange
            var conversationEditCommand = new ConversationEditCommand
            {
                IsActive = true,
                CreatedOn = DateTime.Today,
                ConversationEditRequest = new ConversationEditRequest
                {
                    Description = "test",
                    assignedFiles = new List<ConversationFiles>(),
                    employeeTags = new List<ConversationEmployeeTags> { new ConversationEmployeeTags { EmployeeId = 1 } },
                    ConversationId = 2
                },
                UpdatedOn = DateTime.Today

            };
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = -1,
                    ConversationId = 1,
                    Description = "Conversation",
                    IsActive = true
                }
            };
            var conversationFile = new List<ConversationFile>
            {
                new ConversationFile{
                    CreatedBy = -1,
                    ConversationId = 2,
                    FileName = "Conversation1",
                    StorageFileName = "test1",
                    IsActive = true,
                    ConversationFileId = 1
                }
                // new ConversationFile{
                //    CreatedBy = -1,
                //    ConversationId = 2,
                //    FileName = "Conversation2",
                //    StorageFileName = "test2",
                //    IsActive = true,
                //    ConversationFileId = 2
                //}
            };
            var conversationEmpTag = new List<EmployeeTag>
            {
                new EmployeeTag{
                    CreatedBy = -1,
                    ModuleDetailsId = 1,
                    TagId = 1,
                    EmployeeTagId = 1,
                    IsActive = true
                }
            };
            var identity = new UserIdentity
            {
                EmployeeId = 1
            };
            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

            var mockConfile = conversationFile.AsQueryable().BuildMock();
            mockConversationFile.Setup(x => x.GetQueryable()).Returns(mockConfile);

            var mockConEmpTag = conversationEmpTag.AsQueryable().BuildMock();
            mockConversationEmployeeTag.Setup(x => x.GetQueryable()).Returns(mockConEmpTag);

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
            mockConversation.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).ReturnsAsync(new Conversation { ConversationId = 1 });
            IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
            var ConversationService = ObjConversationService();

            //Act
            var result = ConversationService.Edit(conversationEditCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
        }

        [Fact]
        [Obsolete]
        public void Edit_SuccessConversationEmpTagNotExists()
        {
            //Arrange
            var conversationEditCommand = new ConversationEditCommand
            {

                IsActive = true,
                CreatedOn = DateTime.Today,
                ConversationEditRequest = new ConversationEditRequest
                {
                    IsActive = true,
                    Description = "test",
                    assignedFiles = new List<ConversationFiles> { new ConversationFiles { FileName = "test1", StorageFileName = "test1" } },
                    employeeTags = new List<ConversationEmployeeTags> { new ConversationEmployeeTags { EmployeeId = 2 } },
                    ConversationId = 1,
                    Type = 1
                },
                UpdatedOn = DateTime.Today

            };
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = -1,
                    ConversationId = 1,
                    Description = "Conversation",
                    IsActive = true
                }
            };
            var conversationFile = new List<ConversationFile>
            {
                new ConversationFile{
                    CreatedBy = -1,
                    ConversationId = 1,
                    FileName = "Conversation1",
                    StorageFileName = "test1",
                    IsActive = true,
                    ConversationFileId = 1
                },
                 new ConversationFile{
                    CreatedBy = -1,
                    ConversationId = 2,
                    FileName = "Conversation2",
                    StorageFileName = "test2",
                    IsActive = true,
                    ConversationFileId = 2
                }
            };
            var conversationEmpTag = new List<EmployeeTag>
            {
                new EmployeeTag{
                    CreatedBy = -1,
                    ModuleDetailsId = 1,
                    TagId = 1,
                    EmployeeTagId = 1,
                    IsActive = true
                }
            };
            var identity = new UserIdentity
            {
                EmployeeId = 1
            };
            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

            var mockConfile = conversationFile.AsQueryable().BuildMock();
            mockConversationFile.Setup(x => x.GetQueryable()).Returns(mockConfile);

            var mockConEmpTag = conversationEmpTag.AsQueryable().BuildMock();
            mockConversationEmployeeTag.Setup(x => x.GetQueryable()).Returns(mockConEmpTag);

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
            mockConversation.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).ReturnsAsync(new Conversation { ConversationId = 1 });
            IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
            var ConversationService = ObjConversationService();

            //Act
            var result = ConversationService.Edit(conversationEditCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
        }

        [Fact]
        [Obsolete]
        public void Edit_SuccessConversationOldTagExists()
        {
            //Arrange
            var conversationEditCommand = new ConversationEditCommand
            {

                IsActive = true,
                CreatedOn = DateTime.Today,
                ConversationEditRequest = new ConversationEditRequest
                {
                    IsActive = true,
                    Description = "test",
                    assignedFiles = new List<ConversationFiles> { new ConversationFiles { FileName = "test1", StorageFileName = "test1" } },
                    employeeTags = new List<ConversationEmployeeTags>(),
                    ConversationId = 1,
                    Type = 1
                },
                UpdatedOn = DateTime.Today

            };
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = -1,
                    ConversationId = 1,
                    Description = "Conversation",
                    IsActive = true
                }
            };
            var conversationFile = new List<ConversationFile>
            {
                new ConversationFile{
                    CreatedBy = -1,
                    ConversationId = 1,
                    FileName = "Conversation1",
                    StorageFileName = "test1",
                    IsActive = true,
                    ConversationFileId = 1
                },
                 new ConversationFile{
                    CreatedBy = -1,
                    ConversationId = 2,
                    FileName = "Conversation2",
                    StorageFileName = "test2",
                    IsActive = true,
                    ConversationFileId = 2
                }
            };
            var conversationEmpTag = new List<EmployeeTag>
            {
                new EmployeeTag{
                    CreatedBy = -1,
                    ModuleDetailsId = 1,
                    TagId = 1,
                    EmployeeTagId = 1,
                    IsActive = true
                }
            };
            var identity = new UserIdentity
            {
                EmployeeId = 1
            };
            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

            var mockConfile = conversationFile.AsQueryable().BuildMock();
            mockConversationFile.Setup(x => x.GetQueryable()).Returns(mockConfile);

            var mockConEmpTag = conversationEmpTag.AsQueryable().BuildMock();
            mockConversationEmployeeTag.Setup(x => x.GetQueryable()).Returns(mockConEmpTag);

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
            mockConversation.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).ReturnsAsync(new Conversation { ConversationId = 1 });
            IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
            var ConversationService = ObjConversationService();

            //Act
            var result = ConversationService.Edit(conversationEditCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
        }

        #endregion


        #region Delete
        [Fact]
        [Obsolete]
        public void Delete_NoRecordFound()
        {
            //Arrange
            var conversationDeleteCommand = new ConversationDeleteCommand
            {
                ConversationId = 1
            };
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = -1,
                    ConversationId = 123,
                    Description = "Conversation",
                    IsActive = true
                }
            };
            var identity = new UserIdentity
            {
                EmployeeId = 1
            };
            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);
            IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
            var conversationService = ObjConversationService();

            //Act
            var result = conversationService.DeleteConversation(conversationDeleteCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.Result.Status);
            Assert.False(result.Result.IsSuccess);
        }

        //[Fact]
        //[Obsolete]
        //public void Delete_Success()
        //{ //Arrange
        //    var conversationDeleteCommand = new ConversationDeleteCommand
        //    {
        //        ConversationId = 1
        //    };
        //    var conversationDetails = new List<Conversation>
        //    {
        //        new Conversation{
        //            CreatedBy = -1,
        //            ConversationId = 1,
        //            Description = "Conversation",
        //            IsActive = true
        //        }
        //    };
        //    var conversationFile = new List<ConversationFile>
        //    {
        //        new ConversationFile{
        //            CreatedBy = -1,
        //            ConversationId = 1,
        //            FileName = "Conversation1",
        //            StorageFileName = "test1",
        //            IsActive = true,
        //            ConversationFileId = 1
        //        },
        //         new ConversationFile{
        //            CreatedBy = -1,
        //            ConversationId = 2,
        //            FileName = "Conversation2",
        //            StorageFileName = "test2",
        //            IsActive = true,
        //            ConversationFileId = 2
        //        }
        //    };
        //    var conversationEmpTag = new List<EmployeeTag>
        //    {
        //        new EmployeeTag{
        //            CreatedBy = -1,
        //            ModuleDetailsId = 1,
        //            TagId = 1,
        //            EmployeeTagId = 1,
        //            IsActive = true
        //        }
        //    };
        //    var identity = new UserIdentity
        //    {
        //        EmployeeId = 1
        //    };
        //    var blobVaultResponse = new BlobVaultResponse
        //    {
        //        BlobAccountName = "test",
        //        BlobAccountKey = "test",
        //        BlobContainerName = "test",
        //        BlobCdnCommonUrl = "https://test.com",
        //        BlobCdnUrl = "https://test.com"

        //    };
        //    var stream = new MemoryStream();
        //    var writer = new StreamWriter(stream);
        //    writer.Write("sample data");
        //    writer.Flush();
        //    stream.Position = 0;

        //    var blobMock = new Mock<CloudBlockBlob>(new Uri("http://tempuri.org/blob"));
        //    blobMock.Setup(m => m.DeleteAsync());
        //    //blobMock.Setup(m => m.DownloadToStreamAsync(It.IsAny<Stream>())).Callback((Stream target) => stream.CopyTo(target)).Returns(Task.CompletedTask);

        //    var mock = conversationDetails.AsQueryable().BuildMock();
        //    mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

        //    var mockConfile = conversationFile.AsQueryable().BuildMock();
        //    mockConversationFile.Setup(x => x.GetQueryable()).Returns(mockConfile);

        //    var mockConEmpTag = conversationEmpTag.AsQueryable().BuildMock();
        //    mockConversationEmployeeTag.Setup(x => x.GetQueryable()).Returns(mockConEmpTag);

        //    mockISystemService.Setup(p => p.GetCloudBlockBlob(It.IsAny<string>())).Returns(blobMock.Object);
        //    mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity);           
        //    mockIKeyVaultService.Setup(p => p.GetAzureBlobKeysAsync()).ReturnsAsync(blobVaultResponse);
        //    mockIServicesAggregator.Setup(p => p.Configuration.GetSection("AzureBlob:ConversationImageFolderName").Value).Returns("test");
        //    IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
        //    mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
        //    var conversationService = ObjConversationService();

        //    //Act
        //    var result = conversationService.DeleteConversation(conversationDeleteCommand);

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(200, result.Result.Status);
        //}
        #endregion

        #region GetAllUnreadRecord
        [Fact]
        [Obsolete]
        public void GetAllUnreadConversation_NoRecordFound()
        {
            //Arrange
            var conversationService = ObjConversationService();
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = 1,
                    ConversationId = 1,
                    Description = "Conversation",
                    GoalId = 1,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow
                }
            };
            var conversationlogDetails = new List<ConversationLog>
            {
                new ConversationLog{
                    CreatedBy = 1,
                    EmployeeId = 1,                   
                    IsActive = true,
                    ConversationLastSeenOn = DateTime.UtcNow,
                    CreatedOn = DateTime.UtcNow
                }
            };
            var conversationGetAllQueryRequest = new GetAllUnreadConversationQuery
            {
                EmpId = 1               
            };
            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

            var mockConLog = conversationlogDetails.AsQueryable().BuildMock();
            mockConversationLog.Setup(x => x.GetQueryable()).Returns(mockConLog);

            //Act
            var result = conversationService.GetAllUnreadConversation(conversationGetAllQueryRequest);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        [Obsolete]
        public void GetAllUnreadConversation_IsSuccessTrue()
        {           
            //Arrange
            var conversationService = ObjConversationService();
            var conversationDetails = new List<Conversation>
            {
                new Conversation{
                    CreatedBy = 2,
                    ConversationId = 1,
                    Description = "Conversation",
                    GoalId = 1,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow.AddDays(2)
                }
            };
            var conversationlogDetails = new List<ConversationLog>
            {
                new ConversationLog{
                    CreatedBy = 1,
                    EmployeeId = 1,
                    IsActive = true,
                    ConversationLastSeenOn = DateTime.UtcNow,
                    CreatedOn = DateTime.UtcNow
                }
            };
            var conversationGetAllQueryRequest = new GetAllUnreadConversationQuery
            {
                EmpId = 1
            };
            var mock = conversationDetails.AsQueryable().BuildMock();
            mockConversation.Setup(x => x.GetQueryable()).Returns(mock);

            var mockConLog = conversationlogDetails.AsQueryable().BuildMock();
            mockConversationLog.Setup(x => x.GetQueryable()).Returns(mockConLog);

            //Act
            var result = conversationService.GetAllUnreadConversation(conversationGetAllQueryRequest);
           
            //Assert
            Assert.NotNull(result);
            Assert.True(result.Result.IsSuccess);
        }
        #endregion
    }
}
