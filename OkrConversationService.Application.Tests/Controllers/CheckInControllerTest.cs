using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OkrConversationService.Application.Controllers;
using OkrConversationService.Application.Tests.MockData;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Application.Tests.Controllers
{
    public class CheckInControllerTest
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
        private CheckInController CreateCheckInController()
        {
            Setup();
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            loggerFactory.CreateLogger<CheckInController>();
            return new CheckInController(loggerFactory, _mockMediator.Object, _commonBase.Object);
        }

        #region IsCheck InSubmitted
        [Fact]
        public async Task IsCheckInSubmitted_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateCheckInController();

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<IsCheckInSubmittedQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockCheckInService.MockIsCheckInSubmittedResponse);

            // Act
            var result = await controller.IsCheckInSubmitted();
            var roleResult = ((OkObjectResult)result).Value as Payload<CheckInAlertResponse>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }


        #endregion

        #region Create CheckIn
        [Fact]
        public async Task Create_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateCheckInController();

            var request = new List<CheckInDetailRequest> { new CheckInDetailRequest
            {
                      CheckInPointsId = 1,
                EmployeeId = 123,
                CheckInDetailsId = 1,
                CheckInDetails = "Done"
             }};
            var validationResult = new ValidationResult();

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validationResult));

            _mockMediator.Setup(repo => repo.Send(It.IsAny<CheckInCreateCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockCheckInService.MockCheckInCreateRequest);

            // Act
            var result = await controller.Create(request);
            var roleResult = ((OkObjectResult)result).Value as Payload<CheckInDetailRequest>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }


        #endregion

        #region Get All Check In WeeklyDates
        [Fact]
        public async Task GetAllCheckInWeeklyDates_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateCheckInController();
            long empId = 0;

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<CheckInWeeklyDatesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockCheckInService.MockGetAllCheckInWeeklyResponse);

            // Act
            var result = await controller.GetAllCheckInWeeklyDates(empId);
            var roleResult = ((OkObjectResult)result).Value as Payload<CheckInDatesPermissionResponse>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }


        #endregion

        #region Get DirectReports
        [Fact]
        public async Task GetDirectReports_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateCheckInController();
            long empId = 0;

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<AllDirectReportsEmployeeByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockCheckInService.MockGetAllDirectReportsByIdsResponseResult);

            // Act
            var result = await controller.GetDirectReports(empId);
            var roleResult = ((OkObjectResult)result).Value as Payload<DirectreportsResponseResult>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region Get All
        [Fact]
        public async Task GetAll_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateCheckInController();
            long empId = 0; DateTime? startDate = null; DateTime? endDate = null;

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<CheckInGetAllQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockCheckInService.MockGetAllResponse);

            // Act
            var result = await controller.GetAll(empId, startDate, endDate);
            var roleResult = ((OkObjectResult)result).Value as Payload<CheckInPointsResponse>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetAll_PassValidationTrue_NoRecordFoundExpected_Success()
        {
            // Arrange
            var controller = CreateCheckInController();
            long empId = 0; DateTime? startDate = null; DateTime? endDate = null;

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<CheckInGetAllQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockCheckInService.MockGetNoRecordResponse);

            // Act
            var result = await controller.GetAll(empId, startDate, endDate);
            var roleResult = ((OkObjectResult)result).Value as Payload<CheckInPointsResponse>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);
            Assert.Equal(ResourceMessage.RecordNotFoundMessage, roleResult.MessageList["message"]);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region UpdateCheckinVisibility
        [Fact]
        public async Task UpdateCheckinVisibility_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateCheckInController();
            var checkInVisibilty = 1;

            var validationResult = new ValidationResult();

            //setup
            _validator.Setup(x => x.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(validationResult));

            _mockMediator.Setup(repo => repo.Send(It.IsAny<UpdateCheckinVisibilityCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Payload<CheckInVisible>() { Entity = (CheckInVisible)checkInVisibilty });

            // Act
            var result = await controller.UpdateCheckinVisibility(checkInVisibilty);
            var roleResult = ((OkObjectResult)result).Value as Payload<CheckInVisible>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region Get Dashboard CheckInDetails
        [Fact]
        public async Task GetDashboardCheckInDetails_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateCheckInController();
            long empId = 0;

            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<CheckInDashboardQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(MockCheckInService.MockGetAllDashboardCheckInResponse);

            // Act
            var result = await controller.GetDashboardCheckInDetails(empId);
            var roleResult = ((OkObjectResult)result).Value as Payload<DashboardCheckInResponse>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }
        #endregion

        #region Import
        [Fact]
        public async Task Import_PassValidationTrue_Expected_Success()
        {
            // Arrange
            var controller = CreateCheckInController();


            //setup
            _mockMediator.Setup(repo => repo.Send(It.IsAny<ImportPastTaskCommand>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(new Payload<bool> { Entity = true });

            // Act
            var result = await controller.ImportPastTask();
            var roleResult = ((OkObjectResult)result).Value as Payload<bool>;

            //Assert
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(result);
            Assert.NotNull(((OkObjectResult)result).Value);
            Assert.NotNull(roleResult);

            _mockRepository.VerifyAll();
        }
        #endregion
    }
}
