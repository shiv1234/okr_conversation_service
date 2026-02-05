using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using OkrConversationService.Infrastructure.Services;
using OkrConversationService.Infrastructure.Services.Contracts;
using System;
using Xunit;

namespace OkrConversationService.Infrastructure.Tests.Services
{
    public class KeyVaultServiceTest
    {
        private readonly Mock<IServicesAggregator> _mockIServicesAggregator;
        private readonly Mock<ISystemService> _mockSystemService;       

        public KeyVaultServiceTest()
        {
            _mockIServicesAggregator = new Mock<IServicesAggregator>();
            var mockILoggerFactory = new Mock<ILoggerFactory>();
            
            _mockSystemService = new Mock<ISystemService>();
            _mockIServicesAggregator.Setup(c => c.LoggerFactory).Returns(mockILoggerFactory.Object);
        }
        [Obsolete]
        public KeyVaultService ObjKeyVaultService()
        {
            return new KeyVaultService(_mockIServicesAggregator.Object, _mockSystemService.Object);
        }

        #region GetAzureBlobKeys
       
        [Fact]
        [Obsolete]
        public void GetAzureBlobKeys_IsSuccessTrue()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["TenantId"] = "6+XUkhjhXDVjBiCqODikcVJrho0PY3FwijTIdAdbywQlayt+AbCpwj9WwbVSXPpG";           

            _mockSystemService.Setup(c => c.HttpContext).Returns(httpContext);
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("AzureBlob:BlobAccountKey").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("AzureBlob:BlobAccountName").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("AzureBlob:BlobCdnUrl").Value).Returns("test");

            KeyVaultService keyVaultService = ObjKeyVaultService();

            //Act
            var result = keyVaultService.GetAzureBlobKeysAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.Result.BlobAccountKey);
            Assert.Equal("test", result.Result.BlobAccountName);
            Assert.Equal("test", result.Result.BlobCdnUrl);
            Assert.Equal("testcommon/", result.Result.BlobCdnCommonUrl);
        }
        #endregion
        #region GetSettingsAndUrls
        //[Fact]
        //[Obsolete]
        //public void GetSettingsAndUrls_IsTokenActiveFalse_Null()
        //{
        //    //Arrange
            
        //    KeyVaultService keyVaultService = ObjKeyVaultService();

        //    //Act
        //    var result = keyVaultService.GetSettingsAndUrlsAsync();

        //    //Assert
        //    Assert.Null(result.Result);
        //}
        [Fact]
        [Obsolete]
        public void GetSettingsAndUrls_IsSuccessTrue()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["TenantId"] = "6+XUkhjhXDVjBiCqODikcVJrho0PY3FwijTIdAdbywQlayt+AbCpwj9WwbVSXPpG";
            httpContext.Request.Headers["OriginHost"] = "https:\\test.com";
            

            _mockSystemService.Setup(c => c.HttpContext).Returns(httpContext);
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("OkrService:UnlockLog").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("OkrService:BaseUrl").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("OkrService:UnlockTime").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("ResetPassUrl").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("Notifications:BaseUrl").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("TenantService:BaseUrl").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("OkrFrontendURL:FacebookURL").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("OkrFrontendURL:TwitterUrl").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("OkrFrontendURL:LinkedInUrl").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("OkrFrontendURL:InstagramUrl").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("AzureServiceBus:ConnectionString").Value).Returns("test");
            _mockIServicesAggregator.Setup(p => p.Configuration.GetSection("ReportService:BaseUrl").Value).Returns("test");

            KeyVaultService keyVaultService = ObjKeyVaultService();

            //Act
            var result = keyVaultService.GetSettingsAndUrlsAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.Result.OkrBaseAddress);
            Assert.Equal("test", result.Result.OkrUnlockTime);
            Assert.Equal("test", result.Result.OkrUnlockTime);
            Assert.Equal("test", result.Result.NotificationBaseAddress);
            Assert.Equal("test", result.Result.TenantBaseAddress);
        }
        #endregion
    }
}
