using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OkrConversationService.Application.Controllers;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Application.Tests.MockData;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OkrConversationService.Application.Tests.Controllers
{
    public class ConversationControllerTest
    {
        private MockRepository _mockRepository;
        private Mock<IMediator> _mockMediator;
        private Mock<IValidator> _validator;
        private Mock<ICommonBase> _commonBase;
        private Mock<IFormFile> _formFile;
        private void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mockMediator = _mockRepository.Create<IMediator>();
            _validator = new Mock<IValidator>();
            _commonBase = new Mock<ICommonBase>();
            _formFile = new Mock<IFormFile>();
        }
        private ConversationController CreateConversationController()
        {
            Setup();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            loggerFactory.CreateLogger<ConversationController>();
            return new ConversationController(loggerFactory, _mockMediator.Object, _commonBase.Object);
        }

        #region GetAll Conversations
        [Fact]
        public async Task GetAll_Expected_Success()
        {
            // Arrange
            var controller = CreateConversationController();
            var goalId = 1;
            var goaltypeId = 1;
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<ConversationGetAllQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockConversationService.MockGetAllResponse);

            // Act
            var result = await controller.GetAll(goalId, goaltypeId);
            var objResult = ((OkObjectResult)result).Value as Payload<ConversationResponse>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region UploadConversationImage
        [Fact]
        public async Task UploadConversationImage_Expected_Success()
        {
            // Arrange
            var controller = CreateConversationController();

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<UploadFileCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockConversationService.MockConversationUploadFile);

            // Act
            var result = await controller.UploadConversationImage(_formFile.Object,1);
            var objResult = ((OkObjectResult)result).Value as Payload<string>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region Create Conversations
        [Fact]
        public async Task Create_Expected_Success()
        {
            // Arrange
            var controller = CreateConversationController();

            var request = new ConversationCreateRequest() { Description = "test", GoalTypeId = 498, GoalId = 1, assignedFiles = new List<ConversationFiles>(), ConversationId = 1, employeeTags = new List<ConversationEmployeeTags>(), GoalSourceId = 1, Type = 1 };


            var validationResult = new ValidationResult();

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validationResult));

            _mockMediator.Setup(repo => repo.Send(It.IsAny<ConversationCreateCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockConversationService.MockConversationCreateRequest);

            // Act
            var result = await controller.Create(request);
            var objResult = ((OkObjectResult)result).Value as Payload<ConversationCreateRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Create_Expected_ErrorList()
        {
            // Arrange
            var controller = CreateConversationController();

            var request = new ConversationCreateRequest() { Description = "" };
            var validationResult = new ValidationResult();

            var payloadRequest = new Payload<ConversationCreateRequest>()
            {
                Entity = null,
                IsSuccess = false,
                MessageType = "Error",
                Status = 400,
                MessageList = new Dictionary<string, string> { { "Description", "Required" } }
            };

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(validationResult));

            _commonBase.Setup(x => x.GetPayloadStatus(It.IsAny<Payload<ConversationCreateRequest>>(), It.IsAny<List<ValidationFailure>>()))
              .Returns(payloadRequest);

            // Act
            var result = await controller.Create(request);
            var userobjResult = ((OkObjectResult)result).Value as Payload<ConversationCreateRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(userobjResult);
            Assert.Equal(400, userobjResult.Status);
            Assert.False(userobjResult.IsSuccess);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region Edit Conversations
        [Fact]
        public async Task Edit_Expected_Success()
        {
            // Arrange
            var controller = CreateConversationController();

            var request = new ConversationEditRequest() { Description = "test", assignedFiles = new List<ConversationFiles>(), ConversationId = 1, employeeTags = new List<ConversationEmployeeTags>(), Type = 1 };


            var validationResult = new ValidationResult();

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validationResult));

            _mockMediator.Setup(repo => repo.Send(It.IsAny<ConversationEditCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockConversationService.MockConversationEditRequest);

            // Act
            var result = await controller.Edit(request);
            var objResult = ((OkObjectResult)result).Value as Payload<ConversationEditRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task Edit_Expected_ErrorList()
        {
            // Arrange
            var controller = CreateConversationController();

            var request = new ConversationEditRequest() { Description = "", ConversationId = 1 };
            var validationResult = new ValidationResult();

            var payloadRequest = new Payload<ConversationEditRequest>()
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

            _commonBase.Setup(x => x.GetPayloadStatus(It.IsAny<Payload<ConversationEditRequest>>(), It.IsAny<List<ValidationFailure>>()))
              .Returns(payloadRequest);

            // Act
            var result = await controller.Edit(request);
            var userobjResult = ((OkObjectResult)result).Value as Payload<ConversationEditRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(userobjResult);
            Assert.Equal(400, userobjResult.Status);
            Assert.False(userobjResult.IsSuccess);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region Delete Conversations
        [Fact]
        public async Task DeleteConversatio_Expected_Success()
        {
            // Arrange
            var controller = CreateConversationController();

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<ConversationDeleteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockConversationService.MockPayloadBool);

            // Act
            var result = await controller.Delete(1);
            var objResult = ((OkObjectResult)result).Value as Payload<bool>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);            

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteConversation_Expected_ErrorList()
        {
            // Arrange
            var controller = CreateConversationController();
            var validationResult = new ValidationResult();

            var payloadRequest = new Payload<bool>()
            {
                EntityList = null,
                IsSuccess = false,
                MessageType = "Error",
                Status = 400,
                MessageList = new Dictionary<string, string> { { "ConversationId", "Required" } }
            };

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validationResult));

            _commonBase.Setup(x => x.GetPayloadStatus(It.IsAny<Payload<bool>>(), It.IsAny<List<ValidationFailure>>()))
              .Returns(payloadRequest);

            // Act
            var result = await controller.Delete(0);
            var objResult = ((OkObjectResult)result).Value as Payload<bool>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);
            Assert.Equal(400, objResult.Status);
            Assert.False(objResult.IsSuccess);

            _mockRepository.VerifyAll();
        }

        #endregion

        #region GetAllUnreadConversation
        [Fact]
        public async Task GetAllUnreadConversation_Expected_Success()
        {
            // Arrange
            var controller = CreateConversationController();

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<GetAllUnreadConversationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockConversationService.MockUnreadConversationResponse);

            // Act
            var result = await controller.GetAllUnreadConversation(1);
            var objResult = ((OkObjectResult)result).Value as Payload<UnreadConversationResponse>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region IsEmployeeTag
        [Fact]
        public async Task IsEmployeeTag_Expected_Success()
        {
            // Arrange
            var controller = CreateConversationController();

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<IsEmployeeTagQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockConversationService.MockPayloadBool);

            // Act
            var result = await controller.IsEmployeeTag(1);
            var objResult = ((OkObjectResult)result).Value as Payload<bool>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region CreateLike
        [Fact]
        public async Task CreateLike_Expected_Success()
        {
            // Arrange
            var controller = CreateConversationController();

            var request = new ConversationLikeCreateRequest() {ModuleDetailsId = 1 , EmployeeId = 1 , IsActive = true,ModuleId = 1};

            var validationResult = new ValidationResult();
            var payloadRequest = new Payload<ConversationLikeCreateRequest>();
            

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(validationResult));
            _commonBase.Setup(x => x.GetPayloadStatus(It.IsAny<Payload<ConversationLikeCreateRequest>>(), It.IsAny<List<ValidationFailure>>())).Returns(payloadRequest);
            _mockMediator.Setup(repo => repo.Send(It.IsAny<ConversationLikeCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockConversationService.MockConversationLikeCreateRequest);

            // Act
            var result = await controller.CreateLike(request);
            var objResult = ((OkObjectResult)result).Value as Payload<ConversationLikeCreateRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task CreateLike_Expected_ErrorList()
        {
            // Arrange
            var controller = CreateConversationController();

            var request = new ConversationLikeCreateRequest() { ModuleDetailsId = 0 };

            var validationResult = new ValidationResult();
            var payloadRequest = new Payload<ConversationLikeCreateRequest>()
            {
                EntityList = null,
                IsSuccess = false,
                MessageType = "Error",
                Status = 400,
                MessageList = new Dictionary<string, string> { { "EmployeeID", "Required" } }
            };

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(validationResult));
            _commonBase.Setup(x => x.GetPayloadStatus(It.IsAny<Payload<ConversationLikeCreateRequest>>(), It.IsAny<List<ValidationFailure>>())).Returns(payloadRequest);
            

            // Act
            var result = await controller.CreateLike(request);
            var objResult = ((OkObjectResult)result).Value as Payload<ConversationLikeCreateRequest>;

            //Assert
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);
            Assert.Equal(400, objResult.Status);
            Assert.False(objResult.IsSuccess);

            _mockRepository.VerifyAll();
        }
        #endregion
    }
}
