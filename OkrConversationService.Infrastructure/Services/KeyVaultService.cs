using Microsoft.Extensions.Configuration;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Services
{
    public class KeyVaultService : BaseService, IKeyVaultService
    {
        public ISystemService SystemService { get; set; }

        [Obsolete("")]
        public KeyVaultService(IServicesAggregator servicesAggregateService, ISystemService systemService) : base(servicesAggregateService)
        {
            SystemService = systemService;
        }
        public async Task<BlobVaultResponse> GetAzureBlobKeysAsync()
        {
            BlobVaultResponse blobVaultResponse = new BlobVaultResponse();
            var hasTenant = SystemService.HttpContext.Request.Headers.TryGetValue("TenantId", out var tenantId);
            if ((!hasTenant && SystemService.HttpContext.Request.Host.Value.Contains("localhost")))
                tenantId = Configuration.GetValue<string>("TenantId");

            if (!string.IsNullOrEmpty(tenantId))
            {
                var tenantString = Encryption.DecryptRijndael(tenantId, AppConstants.EncryptionPrivateKey);
                blobVaultResponse.BlobAccountKey = Configuration.GetValue<string>("AzureBlob:BlobAccountKey");
                blobVaultResponse.BlobAccountName = Configuration.GetValue<string>("AzureBlob:BlobAccountName");
                blobVaultResponse.BlobContainerName = tenantString;
                blobVaultResponse.BlobCdnUrl = Configuration.GetValue<string>("AzureBlob:BlobCdnUrl");
                blobVaultResponse.BlobCdnCommonUrl = Configuration.GetValue<string>("AzureBlob:BlobCdnUrl") + "common/";

            }

            return await Task.FromResult(blobVaultResponse);
        }
        public async Task<ServiceSettingUrlResponse> GetSettingsAndUrlsAsync()
        {
            string domain;
            var hasOrigin = SystemService.HttpContext.Request.Headers.TryGetValue("OriginHost", out var origin);
            if (!hasOrigin && SystemService.HttpContext.Request.Host.Value.Contains("localhost"))
                domain = Configuration.GetValue<string>("FrontEndUrl").ToString();
            else
                domain = string.IsNullOrEmpty(origin) ? string.Empty : origin.ToString();
            var settingsResponse = new ServiceSettingUrlResponse
            {
                UnlockLog = Configuration.GetValue<string>("OkrService:UnlockLog"),
                OkrBaseAddress = Configuration.GetValue<string>("OkrService:BaseUrl"),
                OkrUnlockTime = Configuration.GetValue<string>("OkrService:UnlockTime"),
                FrontEndUrl = domain,
                ResetPassUrl = Configuration.GetValue<string>("ResetPassUrl"),
                NotificationBaseAddress = Configuration.GetValue<string>("Notifications:BaseUrl"),
                TenantBaseAddress = Configuration.GetValue<string>("TenantService:BaseUrl"),
                FacebookUrl = Configuration.GetValue<string>("OkrFrontendURL:FacebookURL"),
                TwitterUrl = Configuration.GetValue<string>("OkrFrontendURL:TwitterUrl"),
                LinkedInUrl = Configuration.GetValue<string>("OkrFrontendURL:LinkedInUrl"),
                InstagramUrl = Configuration.GetValue<string>("OkrFrontendURL:InstagramUrl"),

                AzureConnectionString = Configuration.GetValue<string>("AzureServiceBus:ConnectionString"),
                ReportBaseAddress = Configuration.GetValue<string>("ReportService:BaseUrl")
            };
            return await Task.FromResult(settingsResponse);
        }
    }
}
