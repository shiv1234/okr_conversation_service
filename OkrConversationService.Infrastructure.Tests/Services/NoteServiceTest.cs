using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using MockQueryable.Moq;
using Moq;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.RequestModel.Interface;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Services;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.Services
{
    public class NoteServiceTest
    {
        private readonly Mock<IServicesAggregator> mockIServicesAggregator;
        private readonly Mock<INotificationsEmailsService> mockINotificationsEmailsService;
        private readonly Mock<IKeyVaultService> mockIKeyVaultService;
        private readonly Mock<ICommonService> mockICommonService;

        private readonly Mock<TeamGetAllQuery> mockTeamGetAllQuery;
        private readonly Mock<IRepositoryAsync<Note>> mockNote;
        private readonly Mock<IRepositoryAsync<NoteFile>> mockNoteFile;
        private readonly Mock<IRepositoryAsync<NoteEmployeeTag>> mockNoteEmployeeTag;
        private readonly Mock<UserIdentity> mockIUserIdentity;
        private readonly Mock<IConfiguration> mockConfiguration;
        public NoteServiceTest()
        {
            mockIServicesAggregator = new Mock<IServicesAggregator>();
            mockINotificationsEmailsService = new Mock<INotificationsEmailsService>();
            mockIKeyVaultService = new Mock<IKeyVaultService>();
            mockICommonService = new Mock<ICommonService>();

            var iLoggerFactory = new Mock<ILoggerFactory>();
            mockTeamGetAllQuery = new Mock<TeamGetAllQuery>();
            mockNote = new Mock<IRepositoryAsync<Note>>();
            mockNoteFile = new Mock<IRepositoryAsync<NoteFile>>();
            mockNoteEmployeeTag = new Mock<IRepositoryAsync<NoteEmployeeTag>>();
            mockIUserIdentity = new Mock<UserIdentity>();
            mockConfiguration = new Mock<IConfiguration>();

            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<Note>()).Returns(mockNote.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<NoteFile>()).Returns(mockNoteFile.Object);
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<NoteEmployeeTag>()).Returns(mockNoteEmployeeTag.Object);
            mockIServicesAggregator.Setup(c => c.LoggerFactory).Returns(iLoggerFactory.Object);
            mockICommonService.Setup(c => c.GetUserIdentity()).Returns(mockIUserIdentity.Object);
        }

        [Obsolete]
        public NoteService ObjNoteService()
        {
            return new NoteService(mockIServicesAggregator.Object, mockINotificationsEmailsService.Object, mockIKeyVaultService.Object, mockICommonService.Object, mockConfiguration.Object);
        }

        #region GetAll
        [Fact]
        [Obsolete]
        public void GetAll_IsSuccessFalse()
        {
            //Arrange
            var noteService = ObjNoteService();

            //Act
            var result = noteService.GetAll(mockTeamGetAllQuery.Object);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Faulted", result.Status.ToString());
        }

        [Fact]
        [Obsolete]
        public void GetAll_IsSuccessTrue()
        {
            //Arrange
            var noteResponse = new NoteResponse()
            {
                GoalId = 0,
                CreatedBy = 0,
                CreatedOn = DateTime.Now,
                Description = "Test",
                GoalTypeId = 0,
                IsEdited = false,
                IsReadOnly = false,
            };
            var noteresponseObject = new List<NoteResponse>();
            noteresponseObject.Add(noteResponse);

            var noteDetails = new List<Note>()
            {
                new Note(){
                CreatedBy = -1
                }
            };
            var mock = noteDetails.AsQueryable().BuildMock();

            mockNote.Setup(x => x.GetQueryable()).Returns(mock);

            mockICommonService.SetupGet(p => p.noteResponse).Returns(noteresponseObject);


            var noteService = ObjNoteService();

            //Act
            var result = noteService.GetAll(mockTeamGetAllQuery.Object);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Result.IsSuccess);
        }
        #endregion

        #region UploadNotesImageOnAzure
        [Fact]
        [Obsolete]
        public void UploadNotesImageOnAzureExpectedKeyVaultNull_IsSuccessFalse()
        {
            //Arrange
            var uploadFileCommand = new UploadFileCommand();


            mockIKeyVaultService.Setup(c => c.GetAzureBlobKeysAsync()).ReturnsAsync((BlobVaultResponse)null);
            var noteService = ObjNoteService();

            //Act
            var result = noteService.UploadNotesImageOnAzure(uploadFileCommand);

            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        [Obsolete]
        public void UploadNotesImageOnAzureExpectedKeyVaultNotNull_FileLengthZero_IsSuccessFalse()
        {
            //Arrange
            var file = new Mock<IFormFile>();
            var uploadFileCommand = new UploadFileCommand()
            {
                FormFile = file.Object
            };

            var blobVaultResponse = new BlobVaultResponse();

            mockIKeyVaultService.Setup(c => c.GetAzureBlobKeysAsync()).ReturnsAsync(blobVaultResponse);
            var noteService = ObjNoteService();

            //Act
            var result = noteService.UploadNotesImageOnAzure(uploadFileCommand);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.Result.IsSuccess);
        }

        [Fact]
        [Obsolete]
        public void UploadNotesImageOnAzureExpectedKeyVaultNotNull_FileLength_IsSuccessTrue()
        {
            //Arrange
            var filebytes = Encoding.UTF8.GetBytes(@"~\OkrConversationService\OkrConversationService.Infrastructure.Tests\MockData\testImage.jpg");
            IFormFile file = new FormFile(new MemoryStream(filebytes), 0, filebytes.Length, "Data", "testImage.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var uploadFileCommand = new UploadFileCommand()
            {
                FormFile = file
            };
            var blobVaultResponse = new BlobVaultResponse()
            {
                BlobAccountName = "test"
            };

            mockIKeyVaultService.Setup(c => c.GetAzureBlobKeysAsync()).ReturnsAsync(blobVaultResponse);
            mockConfiguration.Setup(p => p.GetSection("AzureBlob:NotesImageFolderName").Value).Returns("test");

            var noteService = ObjNoteService();

            var blobUri = new Uri("http://bogus/myaccount/blob");
            var blobContainer = new Mock<CloudBlobContainer>(MockBehavior.Strict, blobUri);
            mockICommonService
                .Setup(p => p.GetContainerRefByBlobClient(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(blobContainer.Object);

            mockICommonService.Setup(p => p.UploadBlobByLocation(It.IsAny<CloudBlobContainer>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>())).ReturnsAsync(true);

            //Act
            var result = noteService.UploadNotesImageOnAzure(uploadFileCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
        }
        #endregion

        #region Create
        [Fact]
        [Obsolete]
        public void Create_ExpectedAssignedFilesNull_IsSuccess()
        {
            //Arrange
            var noteCreateCommand = new NoteCreateCommand()
            {
                IsActive = true,
                CreatedOn = DateTime.Today,
                NoteCreateRequest = new NoteCreateRequest()
                {
                    Description = "test",
                    NoteId = -1,
                },
                UpdatedOn = DateTime.Today
            };
            var identity = new Mock<UserIdentity>();

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(identity.Object);

            IOperationStatus opStatus = new OperationStatus { Success = false, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);

            var noteService = ObjNoteService();

            //Act
            var result = noteService.Create(noteCreateCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
            Assert.True(result.Result.IsSuccess);
        }

        //[Fact]
        //[Obsolete]
        //public void Create_ExpectedAssignedFilesEmployeeTagsNotNull_IsSuccess()
        //{
        //    //Arrange
        //    var noteFiles = new NoteFiles()
        //    {
        //        FileName = "Test",
        //        StorageFileName = "Test"
        //    };
        //    var mocklistnotefiles = new List<NoteFiles>();
        //    mocklistnotefiles.Add(noteFiles);

        //    var noteEmployeeTags = new NoteEmployeeTags()
        //    {
        //        EmployeeId = 0
        //    };

        //    var listNoteEmployeeTags = new List<NoteEmployeeTags>();
        //    listNoteEmployeeTags.Add(noteEmployeeTags);

        //    var noteCreateCommand = new NoteCreateCommand()
        //    {
        //        IsActive = true,
        //        CreatedOn = DateTime.Today,
        //        NoteCreateRequest = new NoteCreateRequest()
        //        {
        //            Description = "test",
        //            NoteId = -1,
        //            assignedFiles = mocklistnotefiles,
        //            employeeTags = listNoteEmployeeTags
        //        },
        //        UpdatedOn = DateTime.Today
        //    };
        //    var iUserIdentity = new Mock<IUserIdentity>();

        //    mockICommonService.Setup(p => p.GetUserIdentity()).Returns(iUserIdentity.Object);

        //    IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
        //    mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);

        //    var noteService = ObjNoteService();

        //    //Act
        //    var result = noteService.Create(noteCreateCommand);

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(200, result.Result.Status);
        //    Assert.True(result.Result.IsSuccess);
        //}
        #endregion

        #region DeleteNote
        [Fact]
        [Obsolete]
        public void DeleteNote_ExpectedCreatedByDifferntUserNoteNull_IsFailure()
        {
            //Arrange
            var noteDeleteCommand = new NoteDeleteCommand()
            {
                IsActive = true,
                CreatedOn = DateTime.Today,
                UpdatedOn = DateTime.Today
            };
            var iUserIdentity = new Mock<UserIdentity>();

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(iUserIdentity.Object);


            var noteDetails = new List<Note>()
            {
                new Note(){
                CreatedBy = -1
                }
            };
            var mock = noteDetails.AsQueryable().BuildMock();

            mockNote.Setup(x => x.GetQueryable()).Returns(mock);

            IOperationStatus opStatus = new OperationStatus { Success = false, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);

            var noteService = ObjNoteService();

            //Act
            var result = noteService.DeleteNote(noteDeleteCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.Result.Status);
        }

        [Fact]
        [Obsolete]
        public void DeleteNote_ExpectedNoteNotNull_IsSuccess()
        {
            //Arrange
            var noteDeleteCommand = new NoteDeleteCommand()
            {
                NoteId = -1,
            };

            var iUserIdentity = new Mock<UserIdentity>();
            // mock Note repo table
            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(iUserIdentity.Object);
            var noteDetails = new List<Note>()
                {
                    new Note(){
                        NoteId = -1,
                        GoalId = -1,
                        Description = "test",
                        GoalTypeId = -1,
                        IsActive = true,
                        CreatedOn = DateTime.Now
                    }
                };
            var mock = noteDetails.AsQueryable().BuildMock();
            mockNote.Setup(x => x.GetQueryable()).Returns(mock);

            // mock NoteEmployeeTag repo table
            var noteEmployeeTagDetails = new List<NoteEmployeeTag>()
                {
                    new NoteEmployeeTag(){
                        NoteId = -1,
                        IsActive = true,
                        CreatedOn = DateTime.Now
                    }
                };
            var noteEmployeeTag1 = noteEmployeeTagDetails.AsQueryable().BuildMock();
            mockNoteEmployeeTag.Setup(x => x.GetQueryable()).Returns(noteEmployeeTag1);

            // mock NoteFile repo table
            var noteFileDetails = new List<NoteFile>()
                {
                    new NoteFile(){
                        NoteId = -1,
                        IsActive = true,
                        CreatedOn = DateTime.Now
                    }
                };
            var varNoteFileDetails = noteFileDetails.AsQueryable().BuildMock();
            mockNoteFile.Setup(x => x.GetQueryable()).Returns(varNoteFileDetails);

            //Mock KeyVault
            var blobVaultResponse = new BlobVaultResponse()
            {
                BlobAccountName = "test",
                BlobAccountKey = "test"
            };

            mockIKeyVaultService.Setup(c => c.GetAzureBlobKeysAsync()).ReturnsAsync(blobVaultResponse);

            //Mock Configuration
            mockConfiguration.Setup(p => p.GetSection("AzureBlob:NotesImageFolderName").Value).Returns("test");
            // mock SaveChangesAsync
            IOperationStatus opStatus = new OperationStatus { Success = false, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);
            //mock GetContainerRefByBlobClient
            var blobUri = new Uri("http://bogus/myaccount/blob");
            var blobContainer = new Mock<CloudBlobContainer>(MockBehavior.Strict, blobUri);
            mockICommonService.Setup(p => p.GetContainerRefByBlobClient(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(blobContainer.Object);

            //mock DeleteBlobByLocation
            mockICommonService.Setup(p => p.DeleteBlobByLocation(It.IsAny<CloudBlobContainer>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            var noteService = ObjNoteService();

            //Act
            var result = noteService.DeleteNote(noteDeleteCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
            Assert.Equal(-1, result.Result.Entity);
        }
        #endregion

        #region Edit
        [Fact]
        [Obsolete]
        public void Edit_ExpectedSaveChangesAsyncFalse_IsFailure()
        {
            //Arrange
            var noteEditCommand = new NoteEditCommand()
            {
                IsActive = true,
                CreatedOn = DateTime.Today,
                UpdatedOn = DateTime.Today,
                NoteEditRequest = new NoteEditRequest()
                {
                    NoteId = -1,
                }
            };

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(mockIUserIdentity.Object);

            mockNote.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<Note, bool>>>()))
               .ReturnsAsync(new Note() { NoteId = 123 });

            IOperationStatus opStatus = new OperationStatus { Success = false, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);



            var noteService = ObjNoteService();

            //Act
            var result = noteService.Edit(noteEditCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.Result.Status);
        }
        [Fact]
        [Obsolete]
        public void Edit_CoveredElsePart_ExpectedSaveChangesAsyncTrue_IsSuccess()
        {
            //Arrange
            //create List<NoteFiles>
            var files = new NoteFiles()
            {
                FileName = "test",
                StorageFileName = "Test"
            };
            var noteFilesList = new List<NoteFiles>();
            noteFilesList.Add(files);

            //create List<NoteEmployeeTag>
            var noteEmployeeTagsDetails = new List<NoteEmployeeTags>()
                {
                    new NoteEmployeeTags(){
                        EmployeeId = -1
                    }
                };

            var noteEditCommand = new NoteEditCommand()
            {
                IsActive = true,
                CreatedOn = DateTime.Today,
                UpdatedOn = DateTime.Today,
                NoteEditRequest = new NoteEditRequest()
                {
                    NoteId = -1,
                    assignedFiles = noteFilesList,
                    employeeTags = noteEmployeeTagsDetails
                },

            };

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(mockIUserIdentity.Object);

            mockNote.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<Note, bool>>>()))
               .ReturnsAsync(new Note() { NoteId = 123 });

            IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);

            // mock NoteFile repo table
            var noteFileDetails = new List<NoteFile>()
                {
                    new NoteFile(){
                        NoteId = -1,
                        IsActive = true,
                        CreatedOn = DateTime.Now
                    }
                };
            var varNoteFileDetails = noteFileDetails.AsQueryable().BuildMock();
            mockNoteFile.Setup(x => x.GetQueryable()).Returns(varNoteFileDetails);

            // mock NoteEmployeeTag repo table
            var noteEmployeeTagDetails = new List<NoteEmployeeTag>()
                {
                    new NoteEmployeeTag(){
                        NoteId = -1,
                        IsActive = true,
                        CreatedOn = DateTime.Now
                    }
                };
            var noteEmployeeTag1 = noteEmployeeTagDetails.AsQueryable().BuildMock();
            mockNoteEmployeeTag.Setup(x => x.GetQueryable()).Returns(noteEmployeeTag1);
            var noteService = ObjNoteService();

            //mock _notificationsService.UserNotificationsAndEmails
            mockINotificationsEmailsService
                .Setup(p => p.NoteUserNotificationsAndEmails(It.IsAny<List<long>>(),
                It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<string>())).Verifiable();
            //Act
            var result = noteService.Edit(noteEditCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
        }
        [Fact]
        [Obsolete]
        public void Edit_CoveredIfPart_ExpectedSaveChangesAsyncTrue_IsSuccess()
        {
            //Arrange
            //create List<NoteFiles>
            var files = new NoteFiles()
            {
                FileName = "test",
                StorageFileName = "test"
            };
            var noteFilesList = new List<NoteFiles>();
            noteFilesList.Add(files);

            //create List<NoteEmployeeTag>
            var noteEmployeeTagsDetails = new List<NoteEmployeeTags>()
                {
                    new NoteEmployeeTags(){
                        EmployeeId = -1
                    }
                };

            var noteEditCommand = new NoteEditCommand()
            {
                IsActive = true,
                CreatedOn = DateTime.Today,
                UpdatedOn = DateTime.Today,
                NoteEditRequest = new NoteEditRequest()
                {
                    NoteId = -1,
                    assignedFiles = noteFilesList,
                    employeeTags = noteEmployeeTagsDetails,
                    IsActive = true
                },

            };

            mockICommonService.Setup(p => p.GetUserIdentity()).Returns(mockIUserIdentity.Object);

            mockNote.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<Note, bool>>>()))
               .ReturnsAsync(new Note() { NoteId = 123 });

            mockNote.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Note, bool>>>()))
               .ReturnsAsync(new Note() { NoteId = 123 });

            IOperationStatus opStatus = new OperationStatus { Success = true, RecordsAffected = 0 };
            mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.SaveChangesAsync()).ReturnsAsync(opStatus);

            // mock NoteFile repo table
            var noteFileDetails = new List<NoteFile>()
                {
                    new NoteFile(){
                        NoteId = -1,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        StorageFileName = "test"
                    }
                };
            var varNoteFileDetails = noteFileDetails.AsQueryable().BuildMock();
            mockNoteFile.Setup(x => x.GetQueryable()).Returns(varNoteFileDetails);

            mockNoteFile.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<NoteFile, bool>>>()))
               .ReturnsAsync(new NoteFile() { NoteId = 123 });

            // mock NoteEmployeeTag repo table
            var noteEmployeeTagDetails = new List<NoteEmployeeTag>()
                {
                    new NoteEmployeeTag(){
                        NoteId = -1,
                        IsActive = true,
                        CreatedOn = DateTime.Now
                    }
                };
            var noteEmployeeTag1 = noteEmployeeTagDetails.AsQueryable().BuildMock();
            mockNoteEmployeeTag.Setup(x => x.GetQueryable()).Returns(noteEmployeeTag1);
            var noteService = ObjNoteService();

            //mock _notificationsService.UserNotificationsAndEmails
            mockINotificationsEmailsService
                .Setup(p => p.NoteUserNotificationsAndEmails(It.IsAny<List<long>>(),
                It.IsAny<long>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(),It.IsAny<string>())).Verifiable();
            //Act
            var result = noteService.Edit(noteEditCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Result.Status);
        }
        #endregion

        #region IsEmployeeTag
        [Fact]
        [Obsolete]
        public void IsEmployeeTag_IsSuccessFalse()
        {
            //Arrange

            mockICommonService.Setup(c => c.IsNoteEmployeeTag).Returns((NoteEmployeeTag)null);
            var noteService = ObjNoteService();

            //Act
            var result = noteService.IsEmployeeTag(0);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.Result.Entity);
        }
        [Fact]
        [Obsolete]
        public void IsEmployeeTag_IsSuccessTrue()
        {
            //Arrange
            mockICommonService.Setup(c => c.IsNoteEmployeeTag).Returns(new NoteEmployeeTag() { IsActive = true, NoteId = 0 });

            var noteService = ObjNoteService();

            //Act
            var result = noteService.IsEmployeeTag(0);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Result.Entity);
        }
        #endregion
    }
}
