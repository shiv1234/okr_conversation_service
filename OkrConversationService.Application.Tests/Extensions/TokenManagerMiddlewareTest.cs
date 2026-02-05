using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using OkrConversationService.Application.Extensions;
using OkrConversationService.Infrastructure.Services.Contracts;
using System;
using Xunit;

namespace OkrConversationService.Application.Tests.Extensions
{
    public class TokenManagerMiddlewareTest
    {
        private readonly Mock<IConfiguration> mockIConfiguration;
        private readonly Mock<ISystemService> mockISystemService;
        private readonly Mock<RequestDelegate> mockRequestDelegate;

        // Test JWT tokens - these are NOT real tokens, just test data with valid JWT structure
        // Header: {"alg":"HS256","typ":"JWT"} Payload contains test claims
        private const string TestJwtWithEmail = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0LXN1YmplY3QiLCJuYW1lIjoiVGVzdCBVc2VyIiwiaWF0IjoxNjU3MTA0NDgwLCJhdWQiOiJ0ZXN0LWF1ZGllbmNlIiwiaXNzIjoiaHR0cHM6Ly90ZXN0Lmlzc3Vlci5jb20iLCJuYmYiOjE2NTcxMDQ0ODAsImV4cCI6OTk5OTk5OTk5OSwiZW1haWwiOiJ0ZXN0QHRlc3QuY29tIiwib2lkIjoidGVzdC1vaWQiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJ0ZXN0QHRlc3QuY29tIiwidGlkIjoidGVzdC10ZW5hbnQtaWQifQ.test-signature";
        private const string TestJwtWithoutEmail = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0LXN1YmplY3QiLCJuYW1lIjoiVGVzdCBVc2VyIiwiaWF0IjoxNjU3MTA0NDgwLCJhdWQiOiJ0ZXN0LWF1ZGllbmNlIiwiaXNzIjoiaHR0cHM6Ly90ZXN0Lmlzc3Vlci5jb20iLCJuYmYiOjE2NTcxMDQ0ODAsImV4cCI6OTk5OTk5OTk5OSwiZW1haWwiOiIiLCJvaWQiOiJ0ZXN0LW9pZCIsInByZWZlcnJlZF91c2VybmFtZSI6InRlc3RAdGVzdC5jb20iLCJ0aWQiOiJ0ZXN0LXRlbmFudC1pZCJ9.test-signature";
        private const string TestJwtWithoutEmailAndUsername = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0LXN1YmplY3QiLCJuYW1lIjoiVGVzdCBVc2VyIiwiaWF0IjoxNjU3MTA0NDgwLCJhdWQiOiJ0ZXN0LWF1ZGllbmNlIiwiaXNzIjoiaHR0cHM6Ly90ZXN0Lmlzc3Vlci5jb20iLCJuYmYiOjE2NTcxMDQ0ODAsImV4cCI6OTk5OTk5OTk5OSwiZW1haWwiOiIiLCJvaWQiOiJ0ZXN0LW9pZCIsInByZWZlcnJlZF91c2VybmFtZSI6IiIsInVuaXF1ZV9uYW1lIjoidGVzdEB0ZXN0LmNvbSIsInRpZCI6InRlc3QtdGVuYW50LWlkIn0.test-signature";
        private const string TestTokenHeader = "Bearer test-token-value";
        private const string TestTenantIdHeader = "test-encrypted-tenant-id";

        public TokenManagerMiddlewareTest()
        {
            mockIConfiguration = new Mock<IConfiguration>();
            mockISystemService = new Mock<ISystemService>();
            mockRequestDelegate = new Mock<RequestDelegate>();
        }
        [Fact]
        public void TokenManagerMiddleware_HealthPath_IsNull()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/Home/health";
            // Act
            var objResultFilter = new TokenManagerMiddleware(mockIConfiguration.Object, mockISystemService.Object);
            var response = objResultFilter.InvokeAsync(context, mockRequestDelegate.Object);
            // Assert
            Assert.NotNull(response);
            mockIConfiguration.Verify();
        }
        [Fact]
        public void TokenManagerMiddleware_HealthPath_IsError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/Home/Test";
            context.Request.Headers["Authorization"] = "";
            // Act
            var objResultFilter = new TokenManagerMiddleware(mockIConfiguration.Object, mockISystemService.Object);
            var response = objResultFilter.InvokeAsync(context, mockRequestDelegate.Object);
            // Assert
            Assert.NotNull(response);
            mockIConfiguration.Verify();
        }
        [Fact]
        public void TokenManagerMiddleware_APIPath_IsSuccess()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/Home/Index";
            context.Request.Headers["Authorization"] = TestJwtWithEmail;
            context.Request.Headers["Token"] = TestTokenHeader;
            context.Request.Headers["TenantId"] = TestTenantIdHeader;
            context.Request.Headers["OriginHost"] = "https://test.com";

            mockISystemService.Setup(p => p.SystemUri(It.IsAny<string>())).Returns(new Uri("https://test.com"));

            // Act
            var objResultFilter = new TokenManagerMiddleware(mockIConfiguration.Object,
                mockISystemService.Object);
            var response = objResultFilter.InvokeAsync(context, mockRequestDelegate.Object);

            // Assert
            Assert.NotNull(response);
            mockIConfiguration.Verify();
        }
        [Fact]
        public void TokenManagerMiddleware_TokenWithoutEmail_IsNull()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/Home/Index";
            context.Request.Headers["Authorization"] = TestJwtWithoutEmail;
            context.Request.Headers["Token"] = TestTokenHeader;
            context.Request.Headers["TenantId"] = TestTenantIdHeader;
            context.Request.Headers["OriginHost"] = "https://test.com";

            mockISystemService.Setup(p => p.SystemUri(It.IsAny<string>())).Returns(new Uri("https://test.com"));

            // Act
            var objResultFilter = new TokenManagerMiddleware(mockIConfiguration.Object,
                mockISystemService.Object);
            var response = objResultFilter.InvokeAsync(context, mockRequestDelegate.Object);

            // Assert
            Assert.NotNull(response);
            mockIConfiguration.Verify();
        }
        [Fact]
        public void TokenManagerMiddleware_TokenWithoutEmailAndPreferredUsername_IsNull()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/Home/Index";
            context.Request.Headers["Authorization"] = TestJwtWithoutEmailAndUsername;
            context.Request.Headers["Token"] = TestTokenHeader;
            context.Request.Headers["TenantId"] = TestTenantIdHeader;
            context.Request.Headers["OriginHost"] = "https://test.com";

            mockISystemService.Setup(p => p.SystemUri(It.IsAny<string>())).Returns(new Uri("https://test.com"));

            // Act
            var objResultFilter = new TokenManagerMiddleware(mockIConfiguration.Object,
                mockISystemService.Object);
            var response = objResultFilter.InvokeAsync(context, mockRequestDelegate.Object);

            // Assert
            Assert.NotNull(response);
            mockIConfiguration.Verify();
        }
    }
}
