using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OkrConversationService.Application.Controllers;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Tests.MockData;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Application.Tests.Controllers
{
    public class NoteControllerTest
    {
        private MockRepository _mockRepository;
        private Mock<IMediator> _mockMediator;
        private Mock<IValidator> _validator;
        private Mock<ICommonBase> _commonBase;
        private void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockMediator = _mockRepository.Create<IMediator>();
            _validator = new Mock<IValidator>();
            _commonBase = new Mock<ICommonBase>();
        }
        private NoteController CreateNotesController()
        {
            Setup();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            loggerFactory.CreateLogger<NoteController>();
            return new NoteController(loggerFactory, _mockMediator.Object, _commonBase.Object);
        }

        #region Get All
        [Fact]
        public async Task GetAll_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateNotesController();
            var pageIndex = 1;
            var pageSize = 10;
            var GoalId = 1; var GoalTypeId = 1;

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<TeamGetAllQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockNoteService.MockGetAllResponse);

            // Act
            var result = await controller.GetAll(GoalId, GoalTypeId, pageIndex, pageSize);
            var roleResult = ((OkObjectResult)result).Value as Payload<NoteResponse>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }
        #endregion
        #region UploadNotesImage
        [Fact]
        public async Task UploadNotesImage_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateNotesController();

            var mockIFromFile = new Mock<IFormFile>();
            var mockPayloadString = new Mock<Payload<string>>();
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<UploadFileCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockPayloadString.Object);

            // Act
            var result = await controller.UploadNotesImage(mockIFromFile.Object);
            var roleResult = ((OkObjectResult)result).Value as Payload<NoteCreateRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            _mockRepository.VerifyAll();
        }
        #endregion

        #region Create Notes
        [Fact]
        public async Task Create_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateNotesController();
            var employeeTags = new List<NoteEmployeeTags> { new NoteEmployeeTags
            {
                    EmployeeId=1,
             }};
            var assignedFiles = new List<NoteFiles> { new NoteFiles
            {
                    StorageFileName="Test",
                    FileName="Test",
                   FilePath="Test"
            }};
            var request = new NoteCreateRequest() { Description = "test", GoalTypeId = 498, GoalId = 1, assignedFiles = assignedFiles, employeeTags = employeeTags };


            var validationResult = new ValidationResult();

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validationResult));

            _mockMediator.Setup(repo => repo.Send(It.IsAny<NoteCreateCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockNoteService.MockNoteCreateRequest);

            // Act
            var result = await controller.Create(request);
            var roleResult = ((OkObjectResult)result).Value as Payload<NoteCreateRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Create_PassValidationFalse_Expected_ErrorList()
        {
            // Arrange
            var controller = CreateNotesController();
            var employeeTags = new List<NoteEmployeeTags> { new NoteEmployeeTags
            {
                    EmployeeId=1,
             }};
            var assignedFiles = new List<NoteFiles> { new NoteFiles
            {
                    StorageFileName="Test",
                    FileName="Test",
                   FilePath="Test"
            }};
            var request = new NoteCreateRequest() { Description = "", GoalTypeId = 498, GoalId = 1, assignedFiles = assignedFiles, employeeTags = employeeTags };
            var validationResult = new ValidationResult();

            var payloadRequest = new Payload<NoteCreateRequest>()
            {
                EntityList = null,
                IsSuccess = false,
                MessageType = "Error",
                Status = 400,
                MessageList = new Dictionary<string, string> { { "TeamName", "Required" } }
            };

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validationResult));

            _commonBase.Setup(x => x.GetPayloadStatus(It.IsAny<Payload<NoteCreateRequest>>(), It.IsAny<List<ValidationFailure>>()))
              .Returns(payloadRequest);

            // Act
            var result = await controller.Create(request);
            var userRoleResult = ((OkObjectResult)result).Value as Payload<NoteCreateRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(userRoleResult);
            Assert.Equal(400, userRoleResult.Status);
            Assert.False(userRoleResult.IsSuccess);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region Edit Notes
        [Fact]
        public async Task Edit_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateNotesController();
            var employeeTags = new List<NoteEmployeeTags> { new NoteEmployeeTags
            {
                    EmployeeId=1,
             }};
            var assignedFiles = new List<NoteFiles> { new NoteFiles
            {
                    StorageFileName="Test",
                    FileName="Test",
                   FilePath="Test"
            }};
            var request = new NoteEditRequest() { Description = "test", assignedFiles = assignedFiles, employeeTags = employeeTags };

            var validationResult = new ValidationResult();

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validationResult));

            _mockMediator.Setup(repo => repo.Send(It.IsAny<NoteEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockNoteService.MockNoteEditRequest);

            // Act
            var result = await controller.Edit(request);
            var roleResult = ((OkObjectResult)result).Value as Payload<NoteEditRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Edit_PassValidationFalse_Expected_ErrorList()
        {
            // Arrange
            var controller = CreateNotesController();
            var employeeTags = new List<NoteEmployeeTags> { new NoteEmployeeTags
            {
                    EmployeeId=1,
             }};
            var assignedFiles = new List<NoteFiles> { new NoteFiles
            {
                    StorageFileName="Test",
                    FileName="Test",
                   FilePath="Test"
            }};
            var request = new NoteEditRequest() { Description = "", assignedFiles = assignedFiles, employeeTags = employeeTags };
            var validationResult = new ValidationResult();

            var payloadRequest = new Payload<NoteEditRequest>()
            {
                EntityList = null,
                IsSuccess = false,
                MessageType = "Error",
                Status = 400,
                MessageList = new Dictionary<string, string> { { "Description", "Required" } }
            };

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validationResult));

            _commonBase.Setup(x => x.GetPayloadStatus(It.IsAny<Payload<NoteEditRequest>>(), It.IsAny<List<ValidationFailure>>()))
              .Returns(payloadRequest);

            // Act
            var result = await controller.Edit(request);
            var userRoleResult = ((OkObjectResult)result).Value as Payload<NoteEditRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(userRoleResult);
            Assert.Equal(400, userRoleResult.Status);
            Assert.False(userRoleResult.IsSuccess);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region Delete
        [Fact]
        public async Task DeleteNote_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateNotesController();
            long noteid = 1;
            var validationResult = new ValidationResult();

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validationResult));

            _mockMediator.Setup(repo => repo.Send(It.IsAny<NoteDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockNoteService.MockNoteDeleteResponse);

            // Act
            var result = await controller.Delete(noteid);
            var roleResult = ((OkObjectResult)result).Value as Payload<long>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteNote_PassValidationFalse_Expected_ErrorList()
        {
            // Arrange
            var controller = CreateNotesController();
            long noteid = 0;
            var validationResult = new ValidationResult();

            var payloadRequest = new Payload<long>()
            {
                EntityList = null,
                IsSuccess = false,
                MessageType = "Error",
                Status = 400,
                MessageList = new Dictionary<string, string> { { "noteid", "Required" } }
            };

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validationResult));

            _commonBase.Setup(x => x.GetPayloadStatus(It.IsAny<Payload<long>>(), It.IsAny<List<ValidationFailure>>()))
              .Returns(payloadRequest);

            // Act
            var result = await controller.Delete(noteid);
            var userRoleResult = ((OkObjectResult)result).Value as Payload<long>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(userRoleResult);
            Assert.Equal(400, userRoleResult.Status);
            Assert.False(userRoleResult.IsSuccess);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region IsEmployeeTag
        [Fact]
        public async Task IsEmployeeTag_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateNotesController();

            var mockIFromFile = new Mock<IFormFile>();
            var mockPayloadbool = new Mock<Payload<bool>>();
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<IsNoteEmployeeTagQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockPayloadbool.Object);

            // Act
            var result = await controller.IsEmployeeTag(0000);
            var roleResult = ((OkObjectResult)result).Value as Payload<NoteCreateRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            _mockRepository.VerifyAll();
        }
        #endregion
    }
}
