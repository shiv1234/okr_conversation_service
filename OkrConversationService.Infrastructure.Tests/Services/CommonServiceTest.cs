using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Services;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.Services
{
    public class CommonServiceTest
    {
        private readonly Mock<IServicesAggregator> _mockIServicesAggregator;
        private readonly Mock<ILoggerFactory> mockILoggerFactory;
        private readonly Mock<ISystemService> _mockSystemService;
        private readonly Mock<HttpClient> mockHttpClient;
        private readonly Mock<IRepositoryAsync<ConversationFile>> _mockConversationFile;
        private readonly Mock<IRepositoryAsync<EmployeeTeamMapping>> _mockEmployeeTeamMapping;
        private readonly Mock<IRepositoryAsync<Team>> _mockTeam;
        public CommonServiceTest()
        {
            _mockIServicesAggregator = new Mock<IServicesAggregator>();
            mockILoggerFactory = new Mock<ILoggerFactory>();
            _mockSystemService = new Mock<ISystemService>();
            mockHttpClient = new Mock<HttpClient>();
            _mockConversationFile = new Mock<IRepositoryAsync<ConversationFile>>();

            _mockEmployeeTeamMapping = new Mock<IRepositoryAsync<EmployeeTeamMapping>>();
            _mockTeam = new Mock<IRepositoryAsync<Team>>();

            _mockIServicesAggregator.Setup(c => c.LoggerFactory).Returns(mockILoggerFactory.Object);
            _mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<ConversationFile>()).Returns(_mockConversationFile.Object);
            _mockSystemService.Setup(p => p.SystemHttpClient()).Returns(mockHttpClient.Object);

            _mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<EmployeeTeamMapping>())
              .Returns(_mockEmployeeTeamMapping.Object);
            _mockIServicesAggregator.Setup(c => c.UnitOfWorkAsync.RepositoryAsync<Team>())
               .Returns(_mockTeam.Object);
        }

        #region GetHttpClient
        [Fact]
        [Obsolete]
        public void GetHttpClient_HttpClient_Success()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["TenantId"] = "6+XUkhjhXDVjBiCqODikcVJrho0PY3FwijTIdAdbywQlayt+AbCpwj9WwbVSXPpG";
            httpContext.Request.Headers["OriginHost"] = "https:\\localhost:9000.com";
            _mockSystemService.Setup(c => c.HttpContext).Returns(httpContext);

            ICommonService objCommonService = new CommonService(_mockIServicesAggregator.Object, _mockSystemService.Object);

            //Act
            var result = objCommonService.GetHttpClient("http://msdn.microsoft.com/en-us/library/456dfw4f.aspx");

            //Assert
            Assert.NotNull(result.BaseAddress);
            Assert.Equal("http://msdn.microsoft.com/en-us/library/456dfw4f.aspx", result.BaseAddress.ToString());
        }
        #endregion

        #region GetUserIdentity
        [Fact]
        [Obsolete]
        public void GetUserIdentity_hasIdentityTrue_Success()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["UserIdentity"] = "test";
            _mockSystemService.Setup(c => c.HttpContext).Returns(httpContext);

            ICommonService objCommonService = new CommonService(_mockIServicesAggregator.Object, _mockSystemService.Object);

            //Act
            var result = objCommonService.GetUserIdentity();

            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        [Obsolete]
        public void GetUserIdentity_hasIdentityFalse_Success()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["TenantId"] = "6+XUkhjhXDVjBiCqODikcVJrho0PY3FwijTIdAdbywQlayt+AbCpwj9WwbVSXPpG";
            httpContext.Request.Headers["OriginHost"] = "https:\\localhost:9000.com";
            _mockSystemService.Setup(c => c.HttpContext).Returns(httpContext);

            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("OkrUser:BaseUrl").Value).Returns("http://test.aspx");

            var mockUserIdentity = new UserIdentity()
            {
                EmailId = "test@test.com"
            };

            var mockPayloadUserIdentity = new Payload<UserIdentity>()
            {
                Entity = mockUserIdentity
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            // Setup Protected method on HttpMessageHandler mock.
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.StatusCode = System.Net.HttpStatusCode.OK;//Setting statuscode    
                    response.Content = new StringContent(JsonConvert.SerializeObject(mockPayloadUserIdentity)); // configure your response here    
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); //Setting media type for the response    
                    return response;
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _mockSystemService.Setup(p => p.SystemHttpClient()).Returns(httpClient);

            ICommonService objCommonService = new CommonService(_mockIServicesAggregator.Object, _mockSystemService.Object);

            //Act
            var result = objCommonService.GetUserIdentity();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("test@test.com", result.EmailId);
        }
        #endregion

        #region GetMailerTemplate
        [Fact]
        [Obsolete]
        public void GetMailerTemplate_Success()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["TenantId"] = "6+XUkhjhXDVjBiCqODikcVJrho0PY3FwijTIdAdbywQlayt+AbCpwj9WwbVSXPpG";
            httpContext.Request.Headers["OriginHost"] = "https:\\localhost:9000.com";
            _mockSystemService.Setup(c => c.HttpContext).Returns(httpContext);

            var resServiceSettingUrlResponse = new ServiceSettingUrlResponse()
            {
                NotificationBaseAddress = "http://test.test"
            };

            _mockSystemService.Setup(p => p.KeyVaultService.GetSettingsAndUrlsAsync()).ReturnsAsync(resServiceSettingUrlResponse);

            var mockMailerTemplateRequest = new MailerTemplateRequest()
            {
                Subject = "test"
            };

            var mockPayloadMailerTemplateRequest = new Payload<MailerTemplateRequest>()
            {
                Entity = mockMailerTemplateRequest
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            // Setup Protected method on HttpMessageHandler mock.
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                  {
                      HttpResponseMessage response = new HttpResponseMessage();
                      response.StatusCode = System.Net.HttpStatusCode.OK;//Setting statuscode    
                      response.Content = new StringContent(JsonConvert.SerializeObject(mockPayloadMailerTemplateRequest)); // configure your response here    
                      response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); //Setting media type for the response    
                      return response;
                  });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _mockSystemService.Setup(p => p.SystemHttpClient()).Returns(httpClient);

            ICommonService objCommonService = new CommonService(_mockIServicesAggregator.Object, _mockSystemService.Object);

            //Act
            var result = objCommonService.GetMailerTemplate("test");

            //Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.Result.Subject);
        }
        #endregion

        #region Notifications
        [Fact]
        [Obsolete]
        public void Notifications_Success()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["TenantId"] = "6+XUkhjhXDVjBiCqODikcVJrho0PY3FwijTIdAdbywQlayt+AbCpwj9WwbVSXPpG";
            httpContext.Request.Headers["OriginHost"] = "https:\\localhost:9000.com";
            _mockSystemService.Setup(c => c.HttpContext).Returns(httpContext);

            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("OkrUser:BaseUrl").Value).Returns("http://test.aspx");
            _mockSystemService
                .Setup(p => p.SendServiceBusMessageByBusClient(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            ICommonService objCommonService = new CommonService(_mockIServicesAggregator.Object, _mockSystemService.Object);
            var request = new NotificationsRequest()
            {
                AppId = 0
            };
            var settings = new ServiceSettingUrlResponse()
            {
                NotificationBaseAddress = "https:\\localhost:9000.com"
            };
            //Act
            objCommonService.Notifications(request, settings);

            //Assert
            _mockSystemService.Verify();


        }
        #endregion

        #region Notifications
        [Fact]
        [Obsolete]
        public void SendEmail_Success()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["TenantId"] = "6+XUkhjhXDVjBiCqODikcVJrho0PY3FwijTIdAdbywQlayt+AbCpwj9WwbVSXPpG";
            httpContext.Request.Headers["OriginHost"] = "https:\\localhost:9000.com";
            _mockSystemService.Setup(c => c.HttpContext).Returns(httpContext);

            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("OkrUser:BaseUrl").Value).Returns("http://test.aspx");
            _mockSystemService
                .Setup(p => p.SendServiceBusMessageByBusClient(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            ICommonService objCommonService = new CommonService(_mockIServicesAggregator.Object, _mockSystemService.Object);

            var request = new MailRequest()
            {
                Subject = "test"
            };
            var settings = new ServiceSettingUrlResponse()
            {
                NotificationBaseAddress = "https:\\localhost:9000.com"
            };
            //Act
            objCommonService.SendEmail(request, settings);

            //Assert
            _mockSystemService.Verify();
        }
        #endregion

        #region SetClientDetail
        [Fact]
        [Obsolete]
        public void SetClientDetail_Success()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["TenantId"] = "6+XUkhjhXDVjBiCqODikcVJrho0PY3FwijTIdAdbywQlayt+AbCpwj9WwbVSXPpG";
            httpContext.Request.Headers["OriginHost"] = "https:\\localhost:9000.com";
            _mockSystemService.Setup(c => c.HttpContext).Returns(httpContext);

            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("OkrUser:BaseUrl").Value).Returns("http://test.aspx");

            ICommonService objCommonService = new CommonService(_mockIServicesAggregator.Object, _mockSystemService.Object);

            //Act
            var result = objCommonService.SetClientDetail();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("https:\\localhost:9000.com", result.OriginHost);
        }
        #endregion

        #region CallSignalRFunctionForContributors
        [Fact]
        [Obsolete]
        public void CallSignalRFunctionForContributors_SuccessFalse()
        {
            //Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            // Setup Protected method on HttpMessageHandler mock.
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response.StatusCode = System.Net.HttpStatusCode.OK;//Setting statuscode    
                    response.Content = new StringContent(JsonConvert.SerializeObject("test")); // configure your response here    
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); //Setting media type for the response    
                    return response;
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _mockSystemService.Setup(p => p.SystemHttpClient()).Returns(httpClient);

            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("SignalR:SignalRAzureFunctionEndpoint").Value).Returns("http://test.aspx");

            ICommonService objCommonService = new CommonService(_mockIServicesAggregator.Object, _mockSystemService.Object);
            var modelSignalrRequestModel = new SignalrRequestModel()
            {
                BroadcastTopic = "test",
                EmployeeId = 1
            };
            //Act
            var response = objCommonService.CallSignalRFunctionForContributors(modelSignalrRequestModel);

            //Assert
            Assert.NotNull(response);
        }
        #endregion

        [Fact]
        [Obsolete]
        public void EngagementReport()
        {
            //Arrange
            long empId = 1; int type = 1;
            var employeeTeamMapping = new List<EmployeeTeamMapping>
            {
                new EmployeeTeamMapping
                {
                    EmployeeId = 1,
                    TeamId = 2
                }
            };
            List<Team> teams = new List<Team>
            {
                new Team
                {
                    TeamId = 2,
                    TeamHead = 1
                }
            };

            var objEmpTeamsMapping = employeeTeamMapping.AsQueryable().BuildMock();
            _mockEmployeeTeamMapping.Setup(p => p.GetQueryable()).Returns(objEmpTeamsMapping);

            var objTeams = teams.AsQueryable().BuildMock();
            _mockTeam.Setup(p => p.GetQueryable()).Returns(objTeams);

            _mockSystemService.Setup(c => c.KeyVaultService.GetSettingsAndUrlsAsync())
                .ReturnsAsync(new ServiceSettingUrlResponse
                {
                    AzureConnectionString = "Endpoint = sb://test/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=test"
                });

            ICommonService objCommonService = new CommonService(_mockIServicesAggregator.Object, _mockSystemService.Object);
            var result = objCommonService.AuditEngagementReport(new CreateEngagementReportRequest { EmployeeId = empId, EngagementTypeId = type });

            // Assert
            Assert.NotNull(result);
        }

    }
}
