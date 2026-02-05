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
    public class RecognitionControllerTest
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
        private RecognitionController RecognitionController()
        {
            Setup();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            loggerFactory.CreateLogger<RecognitionController>();
            return new RecognitionController(loggerFactory, _mockMediator.Object, _commonBase.Object);
        }
        #region RecognitionLike
        [Fact]
        public async Task GetRecognitionLike_Success()
        {
            // Arrange
            var controller = RecognitionController();
            var moduleDetailsId = 1;
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<RecognitionLikeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.MockGetRecognitionLike);
            // Act
            var result = await controller.GetRecognitionLike(moduleDetailsId,1);
            var objResult = ((OkObjectResult)result).Value as Payload<RecognitionReactionResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);
            _mockRepository.VerifyAll();
        }
        #endregion
        #region GetComments
        [Fact]
        public async Task GetComments_Success()
        {
            // Arrange
            var controller = RecognitionController();
            var moduleDetailsId = 1;
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<GetCommentQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.MockGetCommentResponse);
            // Act
            var result = await controller.GetComments(moduleDetailsId, 1, 1,1);
            var objResult = ((OkObjectResult)result).Value as Payload<CommentResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);
            _mockRepository.VerifyAll();
        }
        #endregion
        #region GetCategory
        [Fact]
        public async Task GetCategory_Success()
        {
            // Arrange
            var controller = RecognitionController();
            var employeeId = 1;
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<RecognitionCategoryGetQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.MockGetRecognitionCategoryResponse);
            // Act
            var result = await controller.GetCategory(employeeId);
            var objResult = ((OkObjectResult)result).Value as Payload<RecognitionCategoryResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);
            _mockRepository.VerifyAll();
        }
        #endregion
        #region GetOrgRecognition
        [Fact]
        public async Task GetOrgRecognition_Success()
        {
            // Arrange
            var controller = RecognitionController();
            OrgRecognitionRequest recognitionRequest = new OrgRecognitionRequest() { 
                endDate=System.DateTime.MaxValue, id=1, isMyPost=false, pageIndex=1, pageSize=100 , RecognitionId=1, searchType=1, startDate=System.DateTime.MinValue };
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<GetOrgRecognitionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.MockGetOrgRecognitionResponse);
            // Act
            var result = await controller.GetOrgRecognition(recognitionRequest);
            var objResult = ((OkObjectResult)result).Value as Payload<OrgRecognitionResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);
            _mockRepository.VerifyAll();
        }
        #endregion
        #region GetMyWallOfFame
        [Fact]
        public async Task GetMyWallOfFame_Success()
        {
            // Arrange
            var controller = RecognitionController();
            MyWallOfFameRequest myWallOfFameRequest = new MyWallOfFameRequest() {
                  Id=1, PageIndex=1, PageSize=10, SearchType=0 
            };
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<MyWallOfFameGetQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.MockMyWallOfFameResponse);
            // Act
            var result = await controller.GetMyWallOfFame(myWallOfFameRequest);
            var objResult = ((OkObjectResult)result).Value as Payload<MyWallOfFameResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(objResult);
            _mockRepository.VerifyAll();
        }
        #endregion
        #region GetTeamsByEmpId
        [Fact]
        public async Task GetTeamsByEmpId_Success()
        {
            // Arrange
            var controller = RecognitionController();
            long empId = 1;
            //setup
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<TeamsByEmpIdGetQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.MockRecognitionByTeamIdResponse);

            // Act
            var result = await controller.GetTeamsByEmpId(empId);
            var objResult = ((OkObjectResult)result).Value as Payload<RecognitionByTeamIdResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);

            _mockRepository.VerifyAll();
        }
        #endregion
        #region GetRecognitionId
        [Fact]
        public async Task GetRecognitionId_Success()
        {
            // Arrange
            var controller = RecognitionController();
            long empId = 1;
            //setup
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<GetRecognitionByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.MockRecognitionResponse);
            // Act
            var result = await controller.GetRecognitionId(empId);
            var objResult = ((OkObjectResult)result).Value as Payload<RecognitionResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            _mockRepository.VerifyAll();
        }
        #endregion
        #region MyWallOfFameDashBoard
        [Fact]
        public async Task MyWallOfFameDashBoard_Success()
        {
            // Arrange
            var controller = RecognitionController();
            long empId = 1;
            //setup
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<MyWallOfFameDashBoardGetQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.MyWallOfFameDashBoardResponse);
            // Act
            var result = await controller.MyWallOfFameDashBoard();
            var objResult = ((OkObjectResult)result).Value as Payload<MyWallOfFameDashBoardResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            _mockRepository.VerifyAll();
        }
        #endregion
        #region EmployeesLeaderBoard
        [Fact]
        public async Task EmployeesLeaderBoard_Success()
        {
            // Arrange
            var controller = RecognitionController();
            long teamId = 1;

            //setup
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<EmployeesLeaderBoardGetQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.EmployeeLeaderBoardResponse);
            // Act
            EmployeesLeaderBoardRequest teamRequest = new EmployeesLeaderBoardRequest()
            {
                Id = teamId,               
            };
            var result = await controller.EmployeesLeaderBoard(teamRequest);
            var objResult = ((OkObjectResult)result).Value as Payload<RecognitionByTeamIdResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            _mockRepository.VerifyAll();
        }
        #endregion
        #region TeamsLeaderBoard
        [Fact]
        public async Task TeamsLeaderBoard_Success()
        {
            // Arrange
            var controller = RecognitionController();
            long teamId = 1;

            RecognitionLeaderBoardRequest request = new RecognitionLeaderBoardRequest
            {
                Id = teamId,               
            };
            //setup
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<TeamsLeaderBoardGetQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.TeamsLeaderBoardResponse);

            var result = await controller.TeamsLeaderBoard(request);
            var objResult = ((OkObjectResult)result).Value as Payload<RecognitionByTeamIdResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            _mockRepository.VerifyAll();
        }
        #endregion
        #region TotalRecognitionByTeamId
        [Fact]
        public async Task TotalRecognitionByTeamId_Success()
        {
            // Arrange
            var controller = RecognitionController();
            long teamId = 1;
            TotalRecognitionByTeamIdRequest request = new TotalRecognitionByTeamIdRequest
            {
                Id = teamId
              
            };
            //setup
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<TotalRecognitionByTeamIdGetQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.TotalRecognitionByTeamIdResponse);
            // Act
            RecognitionTeamRequest teamRequest = new RecognitionTeamRequest()
            {
                TeamId = teamId,              
            };
            var result = await controller.TotalRecognitionByTeamId(teamRequest);
            var objResult = ((OkObjectResult)result).Value as Payload<TotalRecognitionByTeamIdResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            _mockRepository.VerifyAll();
        }
        #endregion
        #region CreateComment
        [Fact]
        public async Task CreateComment_Success()
        {
            // Arrange
            var controller = RecognitionController();

            CommentDetailsRequest request = new CommentDetailsRequest
            {
                CommentDetailsId = 1,
                Comments = "test",
                ModuleDetailsId = 1,
                RecognitionImageRequests = new List<RecognitionImageRequest> {

                     new RecognitionImageRequest { FileName="", GuidFileName="" }
                    }
            };
            //setup
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<CommentCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.CommentDetailsRequest);
            // Act
            var result = await controller.CreateComment(request);
            var objResult = ((OkObjectResult)result).Value as Payload<CommentDetailsRequest>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);

            _mockRepository.VerifyAll();
        }

        #endregion
        #region DeleteComment
        [Fact]
        public async Task DeleteComment_Success()
        {
            // Arrange
            var controller = RecognitionController();

            long CommentDetailsId = 1;
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<CommentDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Payload<bool> { Entity = true });
            // Act
            var result = await controller.DeleteComment(CommentDetailsId);
            var objResult = ((OkObjectResult)result).Value as Payload<bool>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);

            _mockRepository.VerifyAll();
        }

        #endregion

        #region Create
        [Fact]
        public async Task Create_Success()
        {
            // Arrange
            var controller = RecognitionController();
            RecognitionCreateRequest request = new RecognitionCreateRequest
            {
                IsAttachment = true,
                Message = "test",
                RecognitionCategoryId = 1,
                RecognitionCategoryTypeId = 1,
                RecognitionId = 1,
                RecognitionImageRequests = new List<RecognitionImageRequest> {

                    new RecognitionImageRequest { FileName = "", GuidFileName = "" }
                },
                ReceiverRequest = new List<ReceiverRequest>
                {
                    new ReceiverRequest
                    {
                        Id = 1,
                        SearchType = 1
                    }
                },
                RecognitionEmployeeTags = new List<RecognitionEmployeeTags>
                {
                    new RecognitionEmployeeTags
                    {
                        Id = 1,
                        SearchType = 1
                    }
                }

            };

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<RecognitionCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.CommentRequest);
            // Act
            var result = await controller.Create(request);
            var objResult = ((OkObjectResult)result).Value as Payload<CommentDetailsRequest>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);

            _mockRepository.VerifyAll();
        }

        #endregion


        #region Edit
        [Fact]
        public async Task Edit_Success()
        {
            // Arrange
            var controller = RecognitionController();
            RecognitionEditRequest request = new RecognitionEditRequest
            {
                IsAttachment = true,
                Message = "test",
                RecognitionCategoryId = 1,
                RecognitionCategoryTypeId = 1,
                RecognitionId = 1,
                IsContentChange = true,
                RecognitionImageRequests = new List<RecognitionImageRequest> {

                     new RecognitionImageRequest { FileName="", GuidFileName="" }
                    },
                ReceiverRequest = new List<ReceiverRequest>
                {
                    new ReceiverRequest
                    {
                        Id = 1,
                        SearchType = 1
                    }
                },
                RecognitionEmployeeTags = new List<RecognitionEmployeeTags>
                {
                    new RecognitionEmployeeTags
                    {
                        Id = 1,
                        SearchType = 1
                    }
                }
            };

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<RecognitionEditCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.EditRequest);
            // Act
            var result = await controller.Edit(request);
            var objResult = ((OkObjectResult)result).Value as Payload<RecognitionEditRequest>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);

            _mockRepository.VerifyAll();
        }

        #endregion
        #region Delete
        [Fact]
        public async Task Delete_Success()
        {
            // Arrange
            var controller = RecognitionController();
            long recognitionId = 1;
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<RecognitionDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Payload<bool> { Entity = true });
            // Act
            var result = await controller.Delete(recognitionId);
            var objResult = ((OkObjectResult)result).Value as Payload<bool>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);

            _mockRepository.VerifyAll();
        }

        #endregion
        #region OrgRecognition
        [Fact]
        public async Task OrgRecognition_Success()
        {
            // Arrange
            var controller = RecognitionController();
            long empId = 1;
            //setup
            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<GetOrgRecognitionQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MockConversationService.GetOrgRecognition);

            // Act
            var result = await controller.GetOrgRecognition(new OrgRecognitionRequest { id = 1, searchType = 0 });
            var objResult = ((OkObjectResult)result).Value as Payload<OrgRecognitionResponse>;
            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);

            _mockRepository.VerifyAll();
        }
        //[Fact]
        //public async Task GetRecognitionForWall_Success()
        //{
        //    // Arrange
        //    var controller = RecognitionController();
        //    long empId = 1;
        //    //setup
        //    //setup
        //    _mockMediator.Setup(repo => repo.Send(It.IsAny<GetRecognitionForWallQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(MockConversationService.GetRecognitionForWall);

        //    // Act
        //    var result = await controller.GetRecognitionForWall(new RecognitionForWallRequest { id = 1, searchType = 0 });
        //    var objResult = ((OkObjectResult)result).Value as Payload<RecognitionDetailsResponse>;
        //    //Assert
        //    Assert.Equal(200, ((OkObjectResult)result).StatusCode);
        //    Assert.NotNull(result);
        //    Assert.NotNull(((OkObjectResult)result).Value);

        //    _mockRepository.VerifyAll();
        //}
        #endregion

    }
}